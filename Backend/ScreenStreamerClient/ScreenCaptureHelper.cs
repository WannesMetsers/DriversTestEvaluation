using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using static ScreenStreamer;

public static class ScreenCaptureHelper
{

    [DllImport("user32.dll")]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll")]
    static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public static byte[] Capture()
    {
        var width = (int)SystemParameters.PrimaryScreenWidth;
        var height = (int)SystemParameters.PrimaryScreenHeight;

        using var bitmap = new Bitmap(width, height);
        using var g = Graphics.FromImage(bitmap);

        g.CopyFromScreen(0, 0, 0, 0, new System.Drawing.Size(width, height));

        using var ms = new MemoryStream();
        bitmap.Save(ms, ImageFormat.Jpeg);

        return ms.ToArray();
    }

    public static byte[] CaptureWindow(string windowTitle)
    {
        IntPtr hWnd = FindWindow(null, windowTitle);

        if (hWnd == IntPtr.Zero)
            throw new Exception("Window not found");

        GetWindowRect(hWnd, out RECT rect);

        int width = rect.Right - rect.Left;
        int height = rect.Bottom - rect.Top;

        using var bitmap = new Bitmap(width, height);
        using var g = Graphics.FromImage(bitmap);

        g.CopyFromScreen(rect.Left, rect.Top, 0, 0, new System.Drawing.Size(width, height));

        using var ms = new MemoryStream();
        bitmap.Save(ms, ImageFormat.Jpeg);

        return ms.ToArray();
    }
}