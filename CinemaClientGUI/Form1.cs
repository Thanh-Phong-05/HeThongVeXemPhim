using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Drawing;             // để dùng Color, Button, Panel...
using System.Linq;                // để dùng .Select/.Where/.ToList()
using System.Collections.Generic; // để dùng List<string>


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
        await _writer!.WriteLineAsync(payload);

        var resp = await _reader!.ReadLineAsync();
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
        await _writer!.WriteLineAsync(payload);
        var resp = await _reader!.ReadLineAsync();

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
        if (string.IsNullOrEmpty(showId))
        {
            txtOutput.AppendText("⚠ Nhập ShowId trước.\n");
            return;
        }

        var payload = JsonSerializer.Serialize(new { action = "view_seats", showId });
        await _writer!.WriteLineAsync(payload);
        var resp = await _reader!.ReadLineAsync();

        var json = JsonDocument.Parse(resp!);
        if (!json.RootElement.GetProperty("ok").GetBoolean())
        {
            txtOutput.AppendText("❌ Lỗi khi xem ghế.\n");
            return;
        }

        int rows = json.RootElement.GetProperty("rows").GetInt32();
        int cols = json.RootElement.GetProperty("cols").GetInt32();
        var available = json.RootElement.GetProperty("available").EnumerateArray().Select(x => x.GetString() ?? "").ToList();
        var booked = json.RootElement.GetProperty("booked").EnumerateArray().Select(x => x.GetString() ?? "").ToList();

        // đổ bảng Available vs Booked
        var maxLen = Math.Max(available.Count, booked.Count);
        var seatsTable = Enumerable.Range(0, maxLen)
            .Select(i => new
            {
                Available = i < available.Count ? available[i] : "",
                Booked = i < booked.Count ? booked[i] : ""
            })
            .ToList();
        dataGridSeats.DataSource = seatsTable;

        // vẽ sơ đồ ghế
        RenderSeatMap(rows, cols, booked);

        txtOutput.AppendText("✅ Đã load sơ đồ ghế.\n");
    }

    private async void btnBook_Click(object sender, EventArgs e)
    {
        var showId = txtShowId.Text.Trim();
        var seats = txtSeats.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var payload = JsonSerializer.Serialize(new { action = "book", showId, seats });
        await _writer!.WriteLineAsync(payload);
        var resp = await _reader!.ReadLineAsync();

        txtOutput.Text = $"← {resp}";

        await RefreshSeats(showId);
    }

    private async void btnRelease_Click(object sender, EventArgs e)
    {
        var showId = txtShowId.Text.Trim();
        var seats = txtSeats.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var payload = JsonSerializer.Serialize(new { action = "release", showId, seats });
        await _writer!.WriteLineAsync(payload);
        var resp = await _reader!.ReadLineAsync();

        txtOutput.Text = $"← {resp}";

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
            txtOutput.AppendText("❌ Lỗi khi refresh seats.\n");
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

        // vẽ lại sơ đồ ghế
        RenderSeatMap(rows, cols, booked);
    }


    private void dataGridSeats_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dataGridSeats.Columns[e.ColumnIndex].Name == "Available" && e.Value != null && e.Value.ToString() != "")
        {
            e.CellStyle.BackColor = Color.LightGreen;
            e.CellStyle.ForeColor = Color.Black;
        }

        if (dataGridSeats.Columns[e.ColumnIndex].Name == "Booked" && e.Value != null && e.Value.ToString() != "")
        {
            e.CellStyle.BackColor = Color.LightCoral;
            e.CellStyle.ForeColor = Color.White;
        }
    }

    private void RenderSeatMap(int rows, int cols, List<string> booked)
    {
        panelSeats.Controls.Clear();

        int btnSize = 35; // kích thước mỗi ghế
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

                // nếu ghế đã đặt -> tô đỏ, disable
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
                        if (btn.BackColor == Color.LightGreen) // chọn
                        {
                            btn.BackColor = Color.Yellow;
                            txtSeats.Text += (txtSeats.Text.Length > 0 ? "," : "") + seatId;
                        }
                        else if (btn.BackColor == Color.Yellow) // bỏ chọn
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
public class MovieDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }


