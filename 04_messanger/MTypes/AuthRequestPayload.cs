using System.Text;
using System.Text.Json;
using MProto.PayloadBase;

namespace MTypes;

public class AuthRequestPayload : JsonPayload
{
    public string Login { get; set; }
    public string Password { get; set; }
    public AuthRequestPayload(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public override MemoryStream GetStream()
    {
        string json = GetJson();

        byte[] bytes = Encoding.UTF8.GetBytes(json);

        MemoryStream memStream = new MemoryStream();
        memStream.Write(bytes, 0, bytes.Length);

        return memStream;
    }

    public override string GetJson()
    {
        return JsonSerializer.Serialize(this);
    }
}
