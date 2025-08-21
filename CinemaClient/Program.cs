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
    }
}
