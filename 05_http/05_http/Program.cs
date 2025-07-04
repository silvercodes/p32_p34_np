
HttpClient client = new HttpClient();

HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, @"https://google.com");

HttpResponseMessage res = await client.SendAsync(req);

Console.WriteLine($"Status: {res.StatusCode}");

foreach (var header in res.Headers)
{
    Console.Write($"{header.Key}: ");
    foreach (string val in header.Value)
        Console.Write($"{val}\t");

    Console.WriteLine();
}

Console.WriteLine("\n\n\n");

string? body = await res.Content.ReadAsStringAsync();
Console.WriteLine(body);
