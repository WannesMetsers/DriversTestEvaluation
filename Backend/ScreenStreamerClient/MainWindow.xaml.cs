using System.Net.Http;
using System.Windows;

namespace ScreenStreamerClient
{
    public partial class MainWindow : Window
    {
        private ScreenStreamer _streamer;

        public MainWindow()
        {
            InitializeComponent();
            _streamer = new ScreenStreamer(new HttpClient());
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            await _streamer.StartStreamingAsync("https://localhost:7167/api/stream/frame", windowName.Text);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _streamer.Stop();
        }
    }
}