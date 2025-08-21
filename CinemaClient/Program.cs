using System.Net.Sockets;
using System.Text;
using System.Text.Json;


namespace CinemaClient;

    Console.WriteLine(await reader.ReadLineAsync());

    while (true)
    {
        Console.WriteLine("\nChọn thao tác: 1) List Movies 2) List Shows 3) View Seats 4) Book 5) Release 0) Exit");
        Console.Write("> ");
        var key = Console.ReadLine()?.Trim();
        if (key == "0" || key?.ToLower() == "exit") break;
    }


