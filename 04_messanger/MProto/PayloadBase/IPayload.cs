namespace MProto.PayloadBase;

public interface IPayload
{
    public string CType { get; }        // Content type
    public MemoryStream GetStream();
}
