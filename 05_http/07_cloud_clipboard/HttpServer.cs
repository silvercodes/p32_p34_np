using System.Net;
using System.Text;
using System.Text.Json;
using _07_cloud_clipboard.Services;

namespace _07_cloud_clipboard;

// GET /clipboard
// POST /clipboard
// GET /history
internal class HttpServer
{
    private readonly HttpListener listener;
    private readonly IClipboardStorage storage;
    private bool isRunning;

    public HttpServer(string url, IClipboardStorage storage)
    {
        listener = new HttpListener();
        listener.Prefixes.Add(url);
        this.storage = storage;
    }

    public Task Start()
    {
        isRunning = true;
        listener.Start();
        Console.WriteLine($"Server started at {string.Join(',', listener.Prefixes)}");

        return Task.Run(ListenAsync);
    }

    private async Task ListenAsync()
    {
        try
        {
            while (isRunning)
            {
                var ctx = await listener.GetContextAsync();
                _ = Task.Run(() => ProcessRequestAsync(ctx));
            }
        }
        catch(HttpListenerException ex)
        {
            // TODO: handle exception
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    private async Task ProcessRequestAsync(HttpListenerContext ctx)
    {
        try
        {
            var req = ctx.Request;
            var res = ctx.Response;

            // TODO: CORS
            res.Headers.Add("Access-Control-Allow-Origin", "*");
            res.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            res.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            
            // TODO: OPTIONS
            if (req.HttpMethod == "OPTIONS")
            {
                res.StatusCode = 200;
                res.Close();
                return;
            }

            switch (req.Url?.AbsolutePath)
            {
                case "/clipboard":
                    if (req.HttpMethod == "GET")
                        await HandleGetClipboardAsync(res);
                    else if (req.HttpMethod == "POST")
                        await HandlePostClipboardAsync(req, res);
                    else
                        res.StatusCode = 405;
                    break;
                case "/history":
                    if (req.HttpMethod == "GET")
                        await HandleGetHistoryAsync(res);
                    else
                        res.StatusCode = 405;
                    break;
                default:
                    res.StatusCode = 404;
                    res.Close();
                    break;
            }
        }
        catch (Exception ex)
        {
            ctx.Response.StatusCode = 500;
            var buffer = Encoding.UTF8.GetBytes($"{{\"error\": \"{ex.Message}\"}}");
            await ctx.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            ctx.Response.Close();
        }
    }

    private async Task HandleGetClipboardAsync(HttpListenerResponse res)
    {
        var item = storage.GetLastItem();
        res.ContentType = "application/json";

        if (item is null)
        {
            res.StatusCode = 204;
        }
        else
        {
            var json = JsonSerializer.Serialize(item);
            var buffer = Encoding.UTF8.GetBytes(json);
            await res.OutputStream.WriteAsync (buffer, 0, buffer.Length);
        }

        res.Close();
    }
    private async Task HandlePostClipboardAsync(HttpListenerRequest req, HttpListenerResponse res)
    {
        using var reader = new StreamReader(req.InputStream, req.ContentEncoding);
        var content = await reader.ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(content))
        {
            res.StatusCode = 400;
        }
        else
        {
            storage.SaveItem(content);
            res.StatusCode = 200;
        }

        res.Close();
    }
    private async Task HandleGetHistoryAsync(HttpListenerResponse res)
    {
        var history = storage.GetHistory();
        res.ContentType = "application/json";

        var json = JsonSerializer.Serialize(history);
        var buffer = Encoding.UTF8.GetBytes(json);
        await res.OutputStream.WriteAsync(buffer, 0, buffer.Length);

        res.Close();
    }

    public void Stop()
    {
        isRunning = false;
        listener.Stop();
        listener.Close();
        Console.WriteLine("Server stoped");
    }
}
