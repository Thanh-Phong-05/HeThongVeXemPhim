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
}