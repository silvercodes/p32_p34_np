
using _05_mt_server;

await using Server server = new Server("127.0.0.1", 8080);
await server.StartAsync();


