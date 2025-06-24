using System.Net.Sockets;

namespace Server;

internal class Client
{
    private TcpClient tcpClient;

    public Client(TcpClient tcpClient)
    {
        this.tcpClient = tcpClient;
    }

    public void Processing()
    {

    }
}
