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
        await _writer!.WriteLineAsync(JsonSerializer.Serialize(new { action = "list_movies" }));
        string resp = await _reader!.ReadLineAsync() ?? "";
        txtOutput.Text = resp;
    }

    private async void btnListShows_Click(object sender, EventArgs e)
    {
        string movieId = txtMovieId.Text.Trim();
        await _writer!.WriteLineAsync(JsonSerializer.Serialize(new { action = "list_shows", movieId }));
        string resp = await _reader!.ReadLineAsync() ?? "";
        txtOutput.Text = resp;
    }

    private async void btnViewSeats_Click(object sender, EventArgs e)
    {
        string showId = txtShowId.Text.Trim();
        await _writer!.WriteLineAsync(JsonSerializer.Serialize(new { action = "view_seats", showId }));
        string resp = await _reader!.ReadLineAsync() ?? "";
        txtOutput.Text = resp;
    }
    private async void btnBook_Click(object sender, EventArgs e)
    {
        string showId = txtShowId.Text.Trim();
        string[] seats = txtSeats.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        await _writer!.WriteLineAsync(JsonSerializer.Serialize(new { action = "book", showId, seats }));
        string resp = await _reader!.ReadLineAsync() ?? "";
        txtOutput.Text = resp;
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
