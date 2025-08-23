using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace CinemaClientGUI;

public partial class Form1 : Form
{
    private TcpClient? _client;
    private StreamReader? _reader;
    private StreamWriter? _writer;

    public Form1()
    {
        InitializeComponent();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        _client = new TcpClient();
        await _client.ConnectAsync("127.0.0.1", 5000);
        _reader = new StreamReader(_client.GetStream(), Encoding.UTF8);
        _writer = new StreamWriter(_client.GetStream(), new UTF8Encoding(false)) { AutoFlush = true };

        string hello = await _reader.ReadLineAsync() ?? "";
        MessageBox.Show($"Connected: {hello}");
    }

    private async void btnListMovies_Click(object sender, EventArgs e)
    {
        string payload = JsonSerializer.Serialize(new { action = "list_movies" });
        await writer.WriteLineAsync(payload);

        var resp = await reader.ReadLineAsync();
        var doc = JsonDocument.Parse(resp);
        var root = doc.RootElement;

        if (root.GetProperty("ok").GetBoolean())
        {
            var movies = root.GetProperty("movies")
                .EnumerateArray()
                .Select(m => new MovieDto
                {
                    Id = m.GetProperty("Id").GetString(),
                    Title = m.GetProperty("Title").GetString()
                })
                .ToList();

            dataGridMovies.DataSource = movies;
        }
        else
        {
            MessageBox.Show("Lỗi khi lấy danh sách phim");
        }
    }

    private async void btnListShows_Click(object sender, EventArgs e)
    {
        var movieId = txtMovieId.Text.Trim();
        var payload = JsonSerializer.Serialize(new { action = "list_shows", movieId });
        await writer.WriteLineAsync(payload);
        var resp = await reader.ReadLineAsync();

        var json = JsonDocument.Parse(resp);
        var shows = json.RootElement.GetProperty("shows")
            .EnumerateArray()
            .Select(s => new
            {
                Id = s.GetProperty("Id").GetString(),
                MovieId = s.GetProperty("MovieId").GetString(),
                StartTime = s.GetProperty("startTime").GetDateTime()
            })
            .ToList();

        dataGridShows.DataSource = shows;
    }

    private async void btnViewSeats_Click(object sender, EventArgs e)
    {
         var showId = txtShowId.Text.Trim();
        var payload = JsonSerializer.Serialize(new { action = "view_seats", showId });
        await writer.WriteLineAsync(payload);
        var resp = await reader.ReadLineAsync();

        var json = JsonDocument.Parse(resp);
        var available = json.RootElement.GetProperty("available").EnumerateArray().Select(x => x.GetString()).ToList();
        var booked = json.RootElement.GetProperty("booked").EnumerateArray().Select(x => x.GetString()).ToList();

        // ghép thành bảng Available vs Booked
        var maxLen = Math.Max(available.Count, booked.Count);
        var seatsTable = Enumerable.Range(0, maxLen)
            .Select(i => new
            {
                Available = i < available.Count ? available[i] : "",
                Booked = i < booked.Count ? booked[i] : ""
            })
            .ToList();

        dataGridSeats.DataSource = seatsTable;
    }
    private async void btnBook_Click(object sender, EventArgs e)
    {
        var showId = txtShowId.Text.Trim();
        var seats = txtSeats.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var payload = JsonSerializer.Serialize(new { action = "book", showId, seats });
        await writer.WriteLineAsync(payload);
        var resp = await reader.ReadLineAsync();

        txtOutput.Text = $"← {resp}";

        await RefreshSeats(showId);
    }

    private async void btnRelease_Click(object sender, EventArgs e)
    {
        string showId = txtShowId.Text.Trim();
        string[] seats = txtSeats.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        await _writer!.WriteLineAsync(JsonSerializer.Serialize(new { action = "release", showId, seats }));
        string resp = await _reader!.ReadLineAsync() ?? "";
        txtOutput.Text = resp;
    }
}
public class MovieDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }


