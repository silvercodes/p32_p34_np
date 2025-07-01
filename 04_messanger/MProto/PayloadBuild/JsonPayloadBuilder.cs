using System.Text.Json;
using MProto.PayloadBase;

namespace MProto.PayloadBuild;

internal class JsonPayloadBuilder<T> : IPayloadBuilder<T>
    where T : class, IPayload
{
    public T? BuildFromStream(MemoryStream memStream)
    {
        using StreamReader reader = new StreamReader(memStream);

        string json = reader.ReadToEnd();

        return JsonSerializer.Deserialize<T>(json);
    }
}
