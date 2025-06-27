using System.Net.Sockets;
using MProto;
using MProto.PayloadBase;
using MTypes;

const string host = "127.0.0.1";
const int port = 8080;

try
{
    Console.WriteLine("Press Enter to Connect");
    Console.ReadLine();
    using TcpClient client = new TcpClient(host, port);
    using NetworkStream netStream = client.GetStream();

    ProtoMessage<AuthRequestPayload> pm = new ProtoMessage<AuthRequestPayload>()
    {
        Action = "auth"
    };
    pm.SetHeader("key", "86873254");
    pm.SetHeader("u_id", "101");
    pm.SetPayload(new AuthRequestPayload("vasia@mail.com", "qwerty"));

    while(true)
    {
        MemoryStream memoryStream = pm.GetStream();
        Console.WriteLine("Press Enter to Send");
        Console.ReadLine();
        memoryStream.CopyTo(netStream);
    }












    //using TcpClient client = new TcpClient(host, port);
    //using NetworkStream netStream = client.GetStream();

    //int size = 444;
    //netStream.Write(ConvertToBytes(size), 0, 4);

    Console.ReadLine();



}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}

byte[] ConvertToBytes(int val)
{
    byte[] bytes = BitConverter.GetBytes(val);
    if (BitConverter.IsLittleEndian)
        Array.Reverse(bytes);

    return bytes;
}