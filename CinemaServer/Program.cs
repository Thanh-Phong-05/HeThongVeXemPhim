using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Collections.Concurrent;

namespace CinemaServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var server = new TicketServer(IPAddress.Loopback, 5000);
        Console.WriteLine("Server đang khởi động tại 127.0.0.1:5000 ...");
        await server.StartAsync();
    }
}

public class TicketServer
{
    private readonly TcpListener _listener;
    private readonly ConcurrentDictionary<string, Movie> _movies = new();
    private readonly ConcurrentDictionary<string, Show> _shows = new();

    public TicketServer(IPAddress ip, int port)
    {
        _listener = new TcpListener(ip, port);
        SeedData();
    }
    private void SeedData()
    {
        var m1 = new Movie("M1", "Inception");
        var m2 = new Movie("M2", "Interstellar");
        _movies[m1.Id] = m1;
        _movies[m2.Id] = m2;


        // two shows each
        _shows["S1"] = new Show { Id = "S1", MovieId = m1.Id, StartTime = DateTime.Now.AddHours(2) };
        _shows["S2"] = new Show { Id = "S2", MovieId = m1.Id, StartTime = DateTime.Now.AddHours(5) };
        _shows["S3"] = new Show { Id = "S3", MovieId = m2.Id, StartTime = DateTime.Now.AddHours(3) };
        _shows["S4"] = new Show { Id = "S4", MovieId = m2.Id, StartTime = DateTime.Now.AddHours(6) };
    }
    public async Task StartAsync(CancellationToken ct = default)
    {
        _listener.Start();
        Console.WriteLine("Server đang lắng nghe...");
        while (!ct.IsCancellationRequested)
        {
            var client = await _listener.AcceptTcpClientAsync(ct);
            _ = HandleClientAsync(client, ct); // phục vụ nhiều client song song
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken ct)
    {
        Console.WriteLine($" Client kết nối: {client.Client.RemoteEndPoint}");

        client.NoDelay = true;
        client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

        using var stream = client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        using var writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };

        await writer.WriteLineAsync("{\"hello\":true,\"info\":\"One JSON per line\"}");

        try
        {
            while (!ct.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync();
                if (line == null) break;
                var response = ProcessRequest(line);
                await writer.WriteLineAsync(response);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Lỗi client: {ex.Message}");
        }
        finally
        {
            client.Close();
            Console.WriteLine("⬅ Client ngắt kết nối");
        }
    }

    private string ProcessRequest(string line)
    {
        try
        {
            using var doc = JsonDocument.Parse(line);
            var root = doc.RootElement;
            var action = root.GetProperty("action").GetString() ?? "";

            switch (action)
            {
                case "list_movies":
                    return JsonSerializer.Serialize(new { ok = true, movies = _movies.Values.Select(m => new { m.Id, m.Title }) });

                case "list_shows":
                    var movieId = root.GetProperty("movieId").GetString();
                    var shows = _shows.Values.Where(s => s.MovieId == movieId).Select(s => new { s.Id, s.MovieId, startTime = s.StartTime });
                    return JsonSerializer.Serialize(new { ok = true, shows });

                case "view_seats":
                    var showId = root.GetProperty("showId").GetString();
                    if (showId == null || !_shows.TryGetValue(showId, out var show))
                        return JsonSerializer.Serialize(new { ok = false, error = "show_not_found" });

                    var all = show.AllSeats().ToHashSet();
                    var booked = show.Booked.ToArray();
                    var available = all.Except(booked).OrderBy(x => x).ToArray();
                    return JsonSerializer.Serialize(new { ok = true, rows = show.Rows, cols = show.Cols, available, booked });

                case "book":
                    var showId = root.GetProperty("showId").GetString();
                    var seats = root.GetProperty("seats").EnumerateArray().Select(x => x.GetString()!).ToList();
                    if (showId == null || !_shows.TryGetValue(showId, out var show))
                        return JsonSerializer.Serialize(new { ok = false, error = "show_not_found" });

                    List<string> booked = new(); List<string> failed = new();
                    lock (show.SeatLock)
                    {
                        var valid = show.AllSeats().ToHashSet();
                        foreach (var s in seats)
                        {
                            if (!valid.Contains(s) || show.Booked.Contains(s)) failed.Add(s);
                            else { show.Booked.Add(s); booked.Add(s); }
                        }
                    }
                    return JsonSerializer.Serialize(new { ok = failed.Count == 0, booked, failed });

                case "release":
                    var showId = root.GetProperty("showId").GetString();
                    var seats = root.GetProperty("seats").EnumerateArray().Select(x => x.GetString()!).ToList();
                    if (showId == null || !_shows.TryGetValue(showId, out var show))
                        return JsonSerializer.Serialize(new { ok = false, error = "show_not_found" });

                    List<string> released = new();
                    lock (show.SeatLock)
                    {
                        foreach (var s in seats)
                            if (show.Booked.Remove(s)) released.Add(s);
                    }
                    return JsonSerializer.Serialize(new { ok = true, released });

                default:
                    return JsonSerializer.Serialize(new { ok = false, error = "unknown_action" });
            }
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { ok = false, error = "bad_request", detail = ex.Message });
        }
    }


}