using System.Security.Principal;
using MProto.PayloadBase;
using MProto.PayloadBuild;

namespace MProto;

public class ProtoMessage<T>
    where T : class, IPayload
{
    private const char HEADER_SEPARATOR = ':';
    private const string HEADER_PAYLOAD_LEN_KEY = "len";
    private const string HEADER_PAYLOAD_TYPE_KEY = "ctype";     // "content type"

    private IPayloadBuilder<T> payloadBuilder = null!;

    public string Action { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

    public T? Payload { get; private set; }
   
    public MemoryStream? PayloadStream { get; internal set; }
    public int PayloadLength
    {
        get
        {
            Headers.TryGetValue(HEADER_PAYLOAD_LEN_KEY, out string? value);
            if (string.IsNullOrEmpty(value))
                return 0;

            // TODO: check if int
            return Convert.ToInt32(value);
        }
    }

    internal void InitPaylodBuilder()
    {
        payloadBuilder = Headers[HEADER_PAYLOAD_TYPE_KEY] switch
        {
            "json" => new JsonPayloadBuilder<T>(),

            _ => throw new InvalidOperationException()
        };
    }
    
    public void SetHeader(string key, string value)
    {
        Headers[key] = value;
    }
    public void SetHeader(string? headerLine)
    {
        if (headerLine is null)
            return;

        string[] chunks = headerLine.Split(HEADER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        SetHeader(chunks[0], chunks[1]);
    }

    public void SetPayload(T payload)
    {
        Payload = payload;

        PayloadStream = Payload.GetStream();

        Headers[HEADER_PAYLOAD_LEN_KEY] = PayloadStream.Length.ToString();
        Headers[HEADER_PAYLOAD_TYPE_KEY] = Payload.CType;
    }

    public T? GetPayload()
    {
        if (PayloadStream is null)
            return null;

        Payload = payloadBuilder.BuildFromStream(PayloadStream);

        return Payload;
    }

    public MemoryStream GetStream()
    {
        MemoryStream memStream = new MemoryStream();

        memStream.Write(new byte[ProtoMessageBuilder.MESSAGE_LEN_LABLE_SIZE], 0, ProtoMessageBuilder.MESSAGE_LEN_LABLE_SIZE);

        StreamWriter writer = new StreamWriter(memStream);

        writer.WriteLine(Action);
        foreach (KeyValuePair<string, string> h in Headers)
            writer.WriteLine($"{h.Key}:{h.Value}");
        writer.WriteLine();
        writer.Flush();

        if (Payload is not null && PayloadStream is not null)
        {
            PayloadStream.Position = 0;
            PayloadStream.CopyTo(memStream);
        }

        memStream.Position = 0;

        byte[] sizeLableBytes = ConvertToBytes((int)memStream.Length - ProtoMessageBuilder.MESSAGE_LEN_LABLE_SIZE);
        memStream.Write(sizeLableBytes, 0, ProtoMessageBuilder.MESSAGE_LEN_LABLE_SIZE);

        memStream.Position = 0;

        return memStream;
    }

    private byte[] ConvertToBytes(int val)
    {
        byte[] bytes = BitConverter.GetBytes(val);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);

        return bytes;
    }

}
