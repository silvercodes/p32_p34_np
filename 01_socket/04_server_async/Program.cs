

using System.Net;
using System.Net.Sockets;
using System.Text;

const int port = 8080;
// const string serverIp = "172.20.10.5";
const string serverIp = "127.0.0.1";

using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverIp), port);

try
{
    socket.Bind(endPoint);
    socket.Listen(10);

    Console.WriteLine($"Server started at {serverIp}:{port}");

    await RunProcessingAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}


async Task RunProcessingAsync()
{
    while (true)
    {
        Socket remoteSocket = await socket.AcceptAsync();
        Console.WriteLine("Connection established...");

        byte[] buffer = new byte[1024];
        int byteCount = 0;
        StringBuilder messageBuilder = new StringBuilder();
        do
        {
            byteCount = await remoteSocket.ReceiveAsync(buffer);
            messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, byteCount));
        } while (remoteSocket.Available > 0);

        Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {messageBuilder.ToString()}");

        await Task.Delay(2000);

        string response = "Hello from server! All OK!";
        await remoteSocket.SendAsync(Encoding.UTF8.GetBytes(response));

        remoteSocket.Shutdown(SocketShutdown.Both);
        remoteSocket.Close();

        Console.WriteLine("Connection closed");
    }
}

