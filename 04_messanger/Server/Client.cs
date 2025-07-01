using System.Net.Sockets;
using MProto;
using MProto.PayloadBase;
using MTypes;

namespace Server;

internal class Client
{
    private TcpClient tcpClient;
    private NetworkStream netStream = null!;

    public Client(TcpClient tcpClient)
    {
        this.tcpClient = tcpClient;
    }

    public async Task ProcessingAsync()
    {
        try
        {
            netStream = tcpClient.GetStream();

            ProtoMessageBuilder builder = new ProtoMessageBuilder(netStream);

            while (true) 
            {
                ProtoMessage<AuthRequestPayload> protoMessage = await builder.ReceiveAsync<AuthRequestPayload>();

                AuthRequestPayload? p = protoMessage.GetPayload();
            }
        }
        catch (Exception)
        {
            // TODO: handle exception
            throw;
        }
    }
}
