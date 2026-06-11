using System.Net.Http;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        string baseUrl = configuration["Api:BaseUrl"]!;

        Console.WriteLine("Screen streamer started...");

        var streamer = new ScreenStreamer(new HttpClient());

        await streamer.StartStreamingAsync(baseUrl, "");
    }
}