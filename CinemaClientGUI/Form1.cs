using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

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
        var resp = await _reader!.ReadLineAsync();

        using var doc = JsonDocument.Parse(resp!);
        if (!doc.RootElement.GetProperty("ok").GetBoolean())
        {
            MessageBox.Show("Không lấy được danh sách phim");
            return;
        }

        var movies = doc.RootElement.GetProperty("movies")
            .EnumerateArray()
            .Select(m => new {
                Id = m.GetProperty("Id").GetString(),
                Title = m.GetProperty("Title").GetString()
            })
            .ToList();

        dataGridMovies.DataSource = movies;
    }

    private async void btnListShows_Click(object sender, EventArgs e)
    {
        var movieId = txtMovieId.Text.Trim();
        await _writer!.WriteLineAsync(JsonSerializer.Serialize(new { action = "list_shows", movieId }));
        var resp = await _reader!.ReadLineAsync();

        using var doc = JsonDocument.Parse(resp!);
        if (!doc.RootElement.GetProperty("ok").GetBoolean())
        {
            MessageBox.Show("Không lấy được danh sách suất chiếu");
            return;
        }

        var shows = doc.RootElement.GetProperty("shows")
            .EnumerateArray()
            .Select(s => new {
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
        await _writer!.WriteLineAsync(JsonSerializer.Serialize(new { action = "view_seats", showId }));
        var resp = await _reader!.ReadLineAsync();

        using var doc = JsonDocument.Parse(resp!);
        if (!doc.RootElement.GetProperty("ok").GetBoolean())
        {
            MessageBox.Show("Không lấy được ghế");
            return;
        }

        var available = doc.RootElement.GetProperty("available")
            .EnumerateArray().Select(x => x.GetString()).ToList();
        var booked = doc.RootElement.GetProperty("booked")
            .EnumerateArray().Select(x => x.GetString()).ToList();

        var seats = available.Select(a => new { Seat = a, Status = "Available" })
            .Concat(booked.Select(b => new { Seat = b, Status = "Booked" }))
            .OrderBy(x => x.Seat)
            .ToList();

        dataGridSeats.DataSource = seats;

        RenderSeatMap(doc.RootElement.GetProperty("rows").GetInt32(),
                      doc.RootElement.GetProperty("cols").GetInt32(),
                      booked!);
    }

    private async void btnBook_Click(object sender, EventArgs e)
    {
        var showId = txtShowId.Text.Trim();
        var seats = txtSeats.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var payload = JsonSerializer.Serialize(new { action = "book", showId, seats });
        await _writer!.WriteLineAsync(payload);
        var resp = await _reader!.ReadLineAsync();

        if (resp != null)
        {
            using var doc = JsonDocument.Parse(resp);
            bool ok = doc.RootElement.GetProperty("ok").GetBoolean();

            if (ok)
            {
                var booked = doc.RootElement.GetProperty("booked")
                    .EnumerateArray().Select(x => x.GetString()).ToList();
                MessageBox.Show($"✅ Đặt ghế thành công: {string.Join(", ", booked)}", 
                    "Booking Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var failed = doc.RootElement.GetProperty("failed")
                    .EnumerateArray().Select(x => x.GetString()).ToList();
                MessageBox.Show($"❌ Ghế đã bị đặt: {string.Join(", ", failed)}", 
                    "Booking Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        await RefreshSeats(showId);
    }

    private async void btnRelease_Click(object sender, EventArgs e)
    {
        var showId = txtShowId.Text.Trim();
        var seats = txtSeats.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var payload = JsonSerializer.Serialize(new { action = "release", showId, seats });
        await _writer!.WriteLineAsync(payload);
        var resp = await _reader!.ReadLineAsync();

        if (resp != null)
        {
            using var doc = JsonDocument.Parse(resp);
            bool ok = doc.RootElement.GetProperty("ok").GetBoolean();

            if (ok)
            {
                var released = doc.RootElement.GetProperty("released")
                    .EnumerateArray().Select(x => x.GetString()).ToList();
                MessageBox.Show($"✅ Hủy ghế thành công: {string.Join(", ", released)}",
                    "Release Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var failed = doc.RootElement.GetProperty("failed")
                    .EnumerateArray().Select(x => x.GetString()).ToList();
                MessageBox.Show($"❌ Không thể hủy các ghế: {string.Join(", ", failed)}",
                    "Release Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        await RefreshSeats(showId);
    }

    private async Task RefreshSeats(string showId)
    {
        var payload = JsonSerializer.Serialize(new { action = "view_seats", showId });
        await _writer!.WriteLineAsync(payload);
        var resp = await _reader!.ReadLineAsync();

        var json = JsonDocument.Parse(resp!);
        if (!json.RootElement.GetProperty("ok").GetBoolean())
        {
            MessageBox.Show("❌ Lỗi khi refresh seats.");
            return;
        }

        int rows = json.RootElement.GetProperty("rows").GetInt32();
        int cols = json.RootElement.GetProperty("cols").GetInt32();
        var available = json.RootElement.GetProperty("available").EnumerateArray().Select(x => x.GetString() ?? "").ToList();
        var booked = json.RootElement.GetProperty("booked").EnumerateArray().Select(x => x.GetString() ?? "").ToList();

        var maxLen = Math.Max(available.Count, booked.Count);
        var seatsTable = Enumerable.Range(0, maxLen)
            .Select(i => new
            {
                Available = i < available.Count ? available[i] : "",
                Booked = i < booked.Count ? booked[i] : ""
            })
            .ToList();
        dataGridSeats.DataSource = seatsTable;

        RenderSeatMap(rows, cols, booked);
    }

    private void RenderSeatMap(int rows, int cols, List<string> booked)
    {
        panelSeats.Controls.Clear();
        int btnSize = 35;
        int spacing = 5;

        for (int r = 0; r < rows; r++)
        {
            char rowChar = (char)('A' + r);
            for (int c = 1; c <= cols; c++)
            {
                string seatId = $"{rowChar}{c}";

                Button btn = new Button();
                btn.Text = seatId;
                btn.Width = btnSize;
                btn.Height = btnSize;
                btn.Left = c * (btnSize + spacing);
                btn.Top = r * (btnSize + spacing);

                if (booked.Contains(seatId))
                {
                    btn.BackColor = Color.LightCoral;
                    btn.Enabled = false;
                }
                else
                {
                    btn.BackColor = Color.LightGreen;
                    btn.Enabled = true;
                    btn.Click += (s, e) =>
                    {
                        if (btn.BackColor == Color.LightGreen)
                        {
                            btn.BackColor = Color.Yellow;
                            txtSeats.Text += (txtSeats.Text.Length > 0 ? "," : "") + seatId;
                        }
                        else if (btn.BackColor == Color.Yellow)
                        {
                            btn.BackColor = Color.LightGreen;
                            var seats = txtSeats.Text.Split(',').Where(x => x != seatId).ToArray();
                            txtSeats.Text = string.Join(",", seats);
                        }
                    };
                }

                panelSeats.Controls.Add(btn);
            }
        }
    }
}
