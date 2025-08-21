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
        string? payload = key switch
        {
            "1" => JsonSerializer.Serialize(new { action = "list_movies" }),
            "2" => BuildListShows(),
            "3" => BuildViewSeats(),
            "4" => BuildBook(),
            "5" => BuildRelease(),
            _ => null
        };
        if (payload == null) { Console.WriteLine("❓ Lựa chọn không hợp lệ."); continue; }

        await writer.WriteLineAsync(payload);
        var resp = await reader.ReadLineAsync();
        Console.WriteLine($"← {resp}");

    }


