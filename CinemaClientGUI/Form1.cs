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
}
