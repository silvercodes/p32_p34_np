using _07_cloud_clipboard;
using _07_cloud_clipboard.Services;

const string url = "http://localhost:8000/";
IClipboardStorage storage = new MemoryClipboardStorage();
HttpServer server = new HttpServer(url, storage);

await server.Start();
