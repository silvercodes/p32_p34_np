using System.Security.Principal;
using MProto.PayloadBase;

namespace MProto;

public class ProtoMessage<T>
    where T : IPayload
{
    public string Action { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    public T? Payload { get; private set; }




}
