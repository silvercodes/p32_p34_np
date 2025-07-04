
using FileServer;

Server server = new Server()
{
    RootDirectory = @"C:\Users\ThinkPad\Desktop\storage",
};

await server.StartAsync();

Console.ReadLine();