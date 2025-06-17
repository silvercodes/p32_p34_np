using System.Net;
using System.Net.Sockets;
using System.Text;

const string localHost = "172.20.10.5";
const string remoteHost = "172.20.10.5";

Console.Write("Enter a local port: ");
int localPort = Int32.Parse(Console.ReadLine());
Console.Write("Enter a remote port: ");
int remotePort = Int32.Parse(Console.ReadLine());


using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

_ = Task.Run(() =>
{
    socket.Bind(new IPEndPoint(IPAddress.Parse(localHost), localPort));

    try
    {
        while (true)
        {
            byte[] buffer = new byte[65535];            // <---- Размер буфера МАКСИМАЛЬНЫЙ!!!
            int byteCount = 0;
            StringBuilder messageBuilder = new StringBuilder();

            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

            do
            {
                byteCount = socket.ReceiveFrom(buffer, ref remoteEP);
                messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, byteCount));
            } while (socket.Available > 0);

            if (remoteEP is IPEndPoint remoteEPWithInfo)
                Console.Write($"FROM: {remoteEPWithInfo.Address}:{remoteEPWithInfo.Port} ");

            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} > {messageBuilder.ToString()}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");
    }
    finally
    {
        socket.Close();
    }
});


try
{
    while (true)
    {
        Console.Write("\n<<< ");
        string? input = Console.ReadLine();
        if (input is not null)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            socket.SendTo(data, new IPEndPoint(IPAddress.Parse(remoteHost), remotePort));
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
finally
{
    socket.Close();
}
