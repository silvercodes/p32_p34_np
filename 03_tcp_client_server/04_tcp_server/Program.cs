
using System.Net;
using System.Net.Sockets;

const string host = "127.0.0.1";
const int port = 8080;

TcpListener? listener = null;

try
{
    listener = new TcpListener(IPAddress.Parse(host), port);
    listener.Start();

    Console.WriteLine($"Server started at {host}:{port}");

    TcpClient client = listener.AcceptTcpClient();

    using NetworkStream stream = client.GetStream();        // CONNECTED
    using StreamReader reader = new StreamReader(stream);
    using StreamWriter writer = new StreamWriter(stream);

    string? message = reader.ReadLine();                    // RECEIVE
    Console.WriteLine($">>> {message}");

    Thread.Sleep(3000);

    writer.WriteLine("Hello from server");
    writer.Flush();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
finally
{
    listener?.Stop();
}

Console.ReadLine();


