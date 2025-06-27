
namespace MProto.PayloadBase;

public abstract class JsonPayload : IPayload
{
    public abstract MemoryStream GetStream();
    public abstract string GetJson();
}
