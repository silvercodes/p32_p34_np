using System.Net;
using System.Net.Sockets;
using System.Text;

const int serverPort = 8080;
const string serverIp = "127.0.0.1";

using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

try
{
    Console.WriteLine("Press Enter to connect");
    Console.ReadLine();

    socket.Connect(serverEndPoint);

    while (true)
    {
        Console.Write("> ");
        string? message = Console.ReadLine();

        if (message is null)
            continue;

        
        socket.Send(Encoding.UTF8.GetBytes(message));

        byte[] buffer = new byte[1024];
        int byteCount = 0;
        StringBuilder messageBuilder = new StringBuilder();
        do
        {
            byteCount = socket.Receive(buffer);
            messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, byteCount));
        } while (socket.Available > 0);

        Console.WriteLine($"Response: {messageBuilder.ToString()}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
finally
{
    socket.Shutdown(SocketShutdown.Both);
    socket.Close();

    Console.WriteLine("Connection closed");
}