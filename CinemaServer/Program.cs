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


}