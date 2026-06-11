using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("api/stream")]
public class StreamController : ControllerBase
{
    private readonly IFrameBuffer _frameBuffer;

    public StreamController(IFrameBuffer frameBuffer)
    {
        _frameBuffer = frameBuffer;
    }

    [HttpPost("frame")]
    public async Task<IActionResult> ReceiveFrame()
    {
        using var ms = new MemoryStream();
        await Request.Body.CopyToAsync(ms);
        
        _frameBuffer.Update(ms.ToArray()); // store in RAM only
        if (ms.Length < 1000)
        {
            Console.WriteLine("⚠️ Dropping invalid frame: " + ms.Length);
            return Ok();
        }
        return Ok();
    }

    [HttpGet("latest")]
    public IActionResult GetLatest()
    {
        var frame = _frameBuffer.GetLatest();

        if (frame == null)
            return NotFound();

        return File(frame, "image/jpeg");
    }


}