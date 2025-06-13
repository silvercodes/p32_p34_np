using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;

const int serverPort = 8080;
// const string serverIp = "172.20.10.5";
const string serverIp = "127.0.0.1";

using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

try
{
    Console.Write("> ");
    string? message = Console.ReadLine();

    if (message is null)
        throw new ArgumentException("Message is empty");

    socket.Connect(serverEndPoint);                     // BLOCKING
    socket.Send(Encoding.UTF8.GetBytes(message));       // BLOCKING

    byte[] buffer = new byte[1024];
    int byteCount = 0;
    StringBuilder messageBuilder = new StringBuilder();
    do
    {
        byteCount = socket.Receive(buffer);             // BLOCKING
        messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, byteCount));
    } while (socket.Available > 0);

    Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: Response: {messageBuilder.ToString()}");

    socket.Shutdown(SocketShutdown.Both);
    // socket.Close();

    Console.WriteLine("Connection closed");

}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}