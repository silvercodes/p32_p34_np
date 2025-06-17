using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _05_mt_server;

internal class Server: IAsyncDisposable
{
    private int backlog;
    public string Host { get; }
    public int Port { get; }
    public IPEndPoint IPEndPoint { get; set; }
    public Socket ServerSocket { get; }

    public Server(string host, int port, int backlog = 10)
    {
        Host = host;
        Port = port;
        this.backlog = backlog;

        IPEndPoint = new IPEndPoint(IPAddress.Parse(Host), Port);

        ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ServerSocket.Bind(IPEndPoint);
    }

    public async Task StartAsync()
    {
        try
        {
            ServerSocket.Listen(backlog);
            Console.WriteLine($"Server started at {Host}:{Port}");

            await HandleAsync();
        }
        catch (Exception)
        {

            throw;
        }
    }

    private async Task HandleAsync()
    {
        while (true)
        {
            Socket remoteSocket = await ServerSocket.AcceptAsync();

            if (remoteSocket.RemoteEndPoint is IPEndPoint remoteEP)
                await Console.Out.WriteLineAsync($"Connection opened for remote --> {remoteEP.Address}:{remoteEP.Port}");

            _ = Task.Run(() => HandleRemoteSocket(remoteSocket));
        }
    }

    private void HandleRemoteSocket(Socket remoteSocket)
    {
        try
        {
            while (true)
            {
                byte[] buffer = new byte[1024];
                int byteCount = 0;
                StringBuilder messageBuilder = new StringBuilder();
                do
                {
                    byteCount = remoteSocket.Receive(buffer);
                    messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, byteCount));
                } while (remoteSocket.Available > 0);

                string response = messageBuilder.ToString() switch
                {
                    "time" => DateTime.Now.ToShortTimeString(),
                    "date" => DateTime.Now.ToShortDateString(),
                    _ => "Invalid request"
                };

                remoteSocket.Send(Encoding.UTF8.GetBytes(response));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
        finally
        {
            remoteSocket.Shutdown(SocketShutdown.Both);
            remoteSocket.Close();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await Task.Run(() =>
        {
            if (ServerSocket is not null)
                ServerSocket.Dispose();
        });
    }
}
