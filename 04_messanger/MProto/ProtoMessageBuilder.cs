using System.Net.Sockets;
using MProto.PayloadBase;

namespace MProto;

public class ProtoMessageBuilder
{
    internal const int MESSAGE_LEN_LABLE_SIZE = 4;

    private NetworkStream netStream;
    private MemoryStream memStream;

    public ProtoMessageBuilder(NetworkStream netStream)
    {
        this.netStream = netStream;
    }

    public async Task<ProtoMessage<T>> ReceiveAsync<T>()
        where T : IPayload
    {
        // 1. get len
        byte[] bytes = await ReadBytesAsync(ProtoMessageBuilder.MESSAGE_LEN_LABLE_SIZE);
        int readingSize = ConvertToInt(bytes);

        // 2. read len bytes
        memStream = new MemoryStream(readingSize);
        memStream.Write(await ReadBytesAsync(readingSize), 0, readingSize);
        memStream.Position = 0;

        // 3. new ProtoMessage
        ProtoMessage<T> pm = new ProtoMessage<T>();

        // 4. ExtractMetadata
        using StreamReader reader = new StreamReader(memStream);

        ExtractMetadata(pm, reader);
        // 5. Extract payload bytes

        return pm;
    }

    private void ExtractMetadata<T>(ProtoMessage<T> pm, StreamReader reader)
        where T : IPayload
    {
        reader.BaseStream.Position = 0;

        pm.Action = reader.ReadLine();

        string? headerLine;
        while (!string.IsNullOrEmpty(headerLine = reader.ReadLine()))
            pm.SetHeader(headerLine);
    }

    private async Task<byte[]> ReadBytesAsync(int count)
    {
        byte[] bytes = new byte[count];
        await netStream.ReadExactlyAsync(bytes, 0, count);

        return bytes;
    }

    private int ConvertToInt(byte[] bytes)
    {
        if (BitConverter.IsLittleEndian)
            Array.Reverse(bytes);

        return BitConverter.ToInt32(bytes, 0);
    }
}
