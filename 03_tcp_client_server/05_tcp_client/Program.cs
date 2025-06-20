
using System.Net;
using System.Net.Sockets;

const string host = "127.0.0.1";
const int port = 8080;

try
{
    TcpClient client = new TcpClient();
    client.Connect(host, port);

    using NetworkStream stream = client.GetStream();
    using StreamReader reader = new StreamReader(stream);
    using StreamWriter writer = new StreamWriter(stream);

    writer.WriteLine("Hello from client");
    writer.Flush();

    string? response = reader.ReadToEnd();
    Console.WriteLine($"Response: {response}");

    client.Close();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}

Console.ReadLine();


