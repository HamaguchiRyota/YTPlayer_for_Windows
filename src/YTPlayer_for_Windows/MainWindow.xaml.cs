using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.UI.Xaml;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace YTPlayer_for_Windows
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window, IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// メインの処理
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            double dpiScale = WindowHelper.GetWindowDpiScale(this);
            AppWindow.ResizeClient(new SizeInt32(
                (int)(RootGrid.Width * dpiScale),
                (int)(RootGrid.Height * dpiScale)));

            this.Closed += MainWindow_Closed;
        }

        /// <summary>
        /// 
        /// </summary>
        private void MainWindow_Closed(object sender, WindowEventArgs e)
        {
            myButton.Click -= myButton_Click;
            Dispose();
        }

        /// <summary>
        /// "Click Me"ボタンを押したときの処理
        /// </summary>
        private async void myButton_Click(object sender, RoutedEventArgs e)
        {
            string url = myTextBox.Text;
            string embedCode = url.UrlToEmbedCode();
            if (embedCode != null)
            {
                // Ensure the CoreWebView2 is initialized
                await myWebView.EnsureCoreWebView2Async();

                // Use the embed code
                myWebView.NavigateToString(embedCode);
            }
            else
            {
                // Handle invalid URL
                myTextBox.Text = "";
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    /// マネージリソースの解放
                    myButton.Click -= myButton_Click;
                    this.Closed -= MainWindow_Closed;
                }

                /// アンマネージリソースの解放
                /// ここにアンマネージリソースの解放コードを追加

                disposed = true;
            }
        }

        ~MainWindow()
        {
            Dispose(false);
        }

    }

    /// <summary>
    /// URLからYTの埋め込みを生成・表示する処理
    /// </summary>
    internal static class StringExtensions
    {
        static readonly Regex YoutubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:(.*)v(/|=)|(.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);
        static readonly Regex VimeoVideoRegex = new Regex(@"vimeo\.com/(?:.*#|.*/videos/)?([0-9]+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        static internal string? UrlToEmbedCode(this string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                var youtubeMatch = YoutubeVideoRegex.Match(url);

                if (youtubeMatch.Success)
                {
                    return getYoutubeEmbedCode(youtubeMatch.Groups[youtubeMatch.Groups.Count - 1].Value);
                }

                var vimeoMatch = VimeoVideoRegex.Match(url);
                if (vimeoMatch.Success)
                {
                    return getVimeoEmbedCode(vimeoMatch.Groups[1].Value);
                }
            }

            return null;
        }

        const string youtubeEmbedFormat = "<iframe type=\"text/html\" class=\"embed-responsive-item\" width=\"512\" height=\"288\" src=\"https://www.youtube.com/embed/{0}\"></iframe>";

        private static string getYoutubeEmbedCode(string youtubeId)
        {
            return string.Format(youtubeEmbedFormat, youtubeId);
        }

        const string vimeoEmbedFormat = "<iframe src=\"https://player.vimeo.com/video/{0}\" class=\"embed-responsive-item\" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>";
        private static string getVimeoEmbedCode(string vimeoId)
        {
            return string.Format(vimeoEmbedFormat, vimeoId);
        }
    }

    /// <summary>
    /// Windowサイズに関する
    /// </summary>
    internal static class WindowHelper
    {
        [DllImport("User32.dll")]
        private static extern int GetDpiForWindow(nint hwnd);

        public const double DefaultPixelsPerInch = 96D;

        public static double GetWindowDpiScale(Window window)
        {
            nint windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
            return GetDpiForWindow(windowHandle) / DefaultPixelsPerInch;
        }
    }
}
