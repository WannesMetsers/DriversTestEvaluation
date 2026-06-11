public class FrameBuffer : IFrameBuffer
{
    private byte[] _latestFrame;
    private readonly object _lock = new();

    public void Update(byte[] frame)
    {
        lock (_lock)
        {
            _latestFrame = frame;
        }
    }

    public byte[] GetLatest()
    {
        lock (_lock)
        {
            return _latestFrame;
        }
    }
}