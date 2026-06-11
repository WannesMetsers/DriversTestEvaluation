public interface IFrameBuffer
{


    void Update(byte[] frame);
    
    byte[] GetLatest();
    
}