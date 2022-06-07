using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MicaWindow;
using PInvoke;
using Properties;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace MicaVSCode
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--site-per-process –disable-features=SameSiteByDefaultCookies");
            MainWindow = new MainWindow();
            MainWindow.Show();
        }
        static void SetBackdrop(IntPtr handle, BackdropType BackdropType) => SetBackdrop(handle, (int)BackdropType);
        static void SetBackdrop(IntPtr handle, int BackdropType)
        {
            CustomPInvoke.DwmApi.SetWindowAttribute(
                handle,
                CustomPInvoke.DwmApi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
                BackdropType);
        }
    }
}
