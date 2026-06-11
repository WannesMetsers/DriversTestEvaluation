using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;

public class ScreenStreamer
{

  


    private readonly HttpClient _http;
    private bool _running;

    public ScreenStreamer(HttpClient http)
    {
        _http = http;
    }

    public async Task StartStreamingAsync(string url, string windowName)
    {
        _running = true;

        while (_running)
        {
            
            var frame = ScreenCaptureHelper.CaptureWindow(windowName);

            Console.WriteLine("Sending frame: " + frame.Length);

            var content = new ByteArrayContent(frame);
            content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            var response = await _http.PostAsync(url, content);

            Console.WriteLine("Status: " + response.StatusCode);

            await Task.Delay(100); // ~10 FPS
        }
    }
   
    public void Stop()
    {
        _running = false;
    }
}