
namespace MProto.PayloadBase;

public abstract class JsonPayload : IPayload
{
    public string CType => "json";
    public abstract MemoryStream GetStream();
    public abstract string GetJson();
}
