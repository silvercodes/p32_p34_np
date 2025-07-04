﻿

using System.Net;
using System.Net.Sockets;
using System.Text;

const int port = 8080;
// const string serverIp = "172.20.10.5";
const string serverIp = "127.0.0.1";

using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(serverIp), port);

try
{
    socket.Bind(endPoint);
    socket.Listen();

    Console.WriteLine($"Server started at {serverIp}:{port}");

    while(true)
    {
        Socket remoteSocket = socket.Accept();            // BLOCKING
        // 1. Абстракция над установленным соединением с конкретным клиентом
        // 2. Уникальный идентификатор: Локальный IP/PORT + клиентский IP/PORT
        //                      192.168.1.10:8080 + 192.168.1.20:54123
        Console.WriteLine("Connection established...");

        byte[] buffer = new byte[1024];
        int byteCount = 0;
        StringBuilder messageBuilder = new StringBuilder();
        do
        {
            byteCount = remoteSocket.Receive(buffer);
            messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, byteCount));
        } while (remoteSocket.Available > 0);

        Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {messageBuilder.ToString()}");

        Thread.Sleep(2000);

        // string response = "Hello from server! All OK!";

        string response = @"HTTP/1.1 200 OK
Content-Type: text/html; charset=utf-8
Connection: close

<h1 style='color:red;'>Vasia</h1>";


        remoteSocket.Send(Encoding.UTF8.GetBytes(response));

        remoteSocket.Shutdown(SocketShutdown.Both);
        remoteSocket.Close();

        Console.WriteLine("Connection closed");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}


