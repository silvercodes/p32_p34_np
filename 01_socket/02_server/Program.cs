

using System.Net.Sockets;

const int port = 8080;
const string serverIp = "172.20.10.5";

using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


