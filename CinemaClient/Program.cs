using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace CinemaClient;

public class Program
{
    public static async Task Main(string[] args)
    {
        string host = args.Length > 0 ? args[0] : "127.0.0.1";
        int port = args.Length > 1 ? int.Parse(args[1]) : 5000;

        using var client = new TcpClient();
        client.NoDelay = true;
        await client.ConnectAsync(host, port);
        Console.WriteLine($"✅ Kết nối {host}:{port}");

        using var stream = client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        using var writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true };

        Console.WriteLine(await reader.ReadLineAsync());

        while (true)
        {
            Console.WriteLine("\nChọn thao tác:");
            Console.WriteLine("1) List Movies");
            Console.WriteLine("2) List Shows (nhập movieId)");
            Console.WriteLine("3) View Seats (nhập showId)");
            Console.WriteLine("4) Book (nhập showId + seats A1,A2)");
            Console.WriteLine("5) Release (nhập showId + seats)");
            Console.WriteLine("0) Exit");
            Console.Write("> ");

            var k = Console.ReadLine()?.Trim();
            if (k == "0" || k?.ToLower() == "exit") break;

            string? payload = k switch
            {
                "1" => JsonSerializer.Serialize(new { action = "list_movies" }),
                "2" => BuildListShows(),
                "3" => BuildViewSeats(),
                "4" => BuildBook(),
                "5" => BuildRelease(),
                _ => null
            };
            if (payload == null) { Console.WriteLine("Không hợp lệ."); continue; }

            await writer.WriteLineAsync(payload);
            var resp = await reader.ReadLineAsync();
            Console.WriteLine($" {resp}");
        }

        static string BuildListShows()
        {
            Console.Write("movieId: ");
            var movieId = Console.ReadLine()?.Trim();
            return JsonSerializer.Serialize(new { action = "list_shows", movieId });
        }
        
        static string BuildViewSeats()
        {
            Console.Write("showId: ");
            var showId = Console.ReadLine()?.Trim();
            return JsonSerializer.Serialize(new { action = "view_seats", showId });
        }
    }
}
