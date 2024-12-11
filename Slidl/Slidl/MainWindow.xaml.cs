using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using System.Runtime.InteropServices;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Slidl
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            double dpiScale = WindowHelper.GetWindowDpiScale(this);
            AppWindow.ResizeClient(new SizeInt32(
                (int)(RootGrid.Width * dpiScale),
                (int)(RootGrid.Height * dpiScale)));
        }

        
        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            //myButton.Content = "Clicked";
        }
        

    }

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
