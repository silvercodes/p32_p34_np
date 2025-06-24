using System.Net.Sockets;
using MProto.PayloadBase;

namespace MProto;

internal class ProtoMessageBuilder
{
    private NetworkStream netStream;
    private MemoryStream memStream;

    public ProtoMessageBuilder(NetworkStream netStream)
    {
        this.netStream = netStream;
    }

    public ProtoMessage<T> Receive<T>()
        where T : IPayload
    {

    }
}
