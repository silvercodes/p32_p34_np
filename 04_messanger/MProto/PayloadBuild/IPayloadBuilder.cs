using MProto.PayloadBase;

namespace MProto.PayloadBuild;

public interface IPayloadBuilder<T>
    where T : class, IPayload
{
    public T? BuildFromStream(MemoryStream stream);
}
