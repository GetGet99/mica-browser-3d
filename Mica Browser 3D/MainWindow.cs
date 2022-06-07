extern alias WV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using PInvoke;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Path = System.IO.Path;
using System.IO;
using Timer = System.Windows.Forms.Timer;
using WinFormsCursor = System.Windows.Forms.Cursor;
using WinFormsControl = System.Windows.Forms.Control;
using WV2::Microsoft.Web.WebView2.Core;
using System.Net.Http;

namespace MicaVSCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MicaBrowser.MicaBrowser
    {
        bool _MouseLock;
        bool MouseLock
        {
            get => _MouseLock;
            set
            {
                if (value)
                {
                    var centerX = WebView2.ActualWidth / 2;
                    var centerY = WebView2.ActualHeight / 2 + 30;
                    if (WindowState == WindowState.Normal)
                    {
                        centerX += Left;
                        centerY += Top;
                    }
                    WinFormsCursor.Position = new System.Drawing.Point((int)centerX, (int)centerY);
                }
                _MouseLock = value;
            }
        }
        public MainWindow()
        {
#if WINDOWS10_0_17763_0_OR_GREATER
            Settings.MicaWindowSettings.BackdropType = MicaWindow.BackdropType.Mica;
#endif
            TitleBarContainer.Visibility = Visibility.Visible;
            
            WebView2.CoreWebView2InitializationCompleted += (_, _) =>
            {
                WebView2.CoreWebView2.SetVirtualHostNameToFolderMapping("local.3d.co", @".\web", CoreWebView2HostResourceAccessKind.Allow);
                WebView2.Source = new Uri("http://local.3d.co/index.html");
                //WebView2.Source = new Uri("edge://flags");
                TitleBarContainer.Visibility = Visibility.Collapsed;
                WebView2.CoreWebView2.FrameCreated += (_, e) =>
                {
                    e.Frame.NavigationStarting += (_, e1) =>
                    {
                        e1.AdditionalAllowedFrameAncestors = "*";
                    };
                };
                
                Timer t = new()
                {
                    Interval = 16
                };
                t.Tick += delegate
                {
                    string keyStr = "";
                    double dx = 0, dy = 0;
                    if (IsActive)
                    {
                        if (MouseLock)
                        {
                            if (Keyboard.IsKeyDown(Key.W))
                                keyStr += "w ";
                            if (Keyboard.IsKeyDown(Key.A))
                                keyStr += "a ";
                            if (Keyboard.IsKeyDown(Key.S))
                                keyStr += "s ";
                            if (Keyboard.IsKeyDown(Key.D))
                                keyStr += "d ";
                            if (Keyboard.IsKeyDown(Key.Space))
                                keyStr += "space ";
                            if (Keyboard.IsKeyDown(Key.LeftShift) | Keyboard.IsKeyDown(Key.RightAlt))
                                keyStr += "shift ";
                            if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                                keyStr += "alt ";

                            var pos = WinFormsCursor.Position;
                            var centerX = WebView2.ActualWidth / 2;
                            var centerY = WebView2.ActualHeight / 2 + 30;
                            if (WindowState == WindowState.Normal)
                            {
                                centerX += Left;
                                centerY += Top;
                            }
                            dx = (int)centerX - pos.X;
                            dy = (int)centerY - pos.Y;
                            WinFormsCursor.Position = new System.Drawing.Point((int)centerX, (int)centerY);
                        }
                        
                    }
                    if (keyStr.Length > 0) keyStr = keyStr[..^1];
                    string json = @$"
{{
    ""keys"": [{string.Join(',', keyStr.Split(' ').Select(x => $"\"{x}\""))}],
    ""cursorLock"": {(MouseLock ? "true" : "false")},
    ""mouse"": {{
        ""dX"": {dx},
        ""dY"": {dy},
        ""LeftDown"": {(WinFormsControl.MouseButtons.HasFlag(MouseButtons.Left) ? "true" : "false")},
        ""RightDown"": {(WinFormsControl.MouseButtons.HasFlag(MouseButtons.Right) ? "true" : "false")}
    }}
}}".Trim();
                    WebView2.CoreWebView2.PostWebMessageAsJson(json);
                    WebView2.CoreWebView2.NavigationStarting += (o, e) =>
                    {
                        if (e.Uri != "http://local.3d.co/index.html")
                        {
                            //e.Cancel = true;
                            //WebView2.CoreWebView2.ExecuteScriptAsync($"CreateIFrameInFront(\"{e.Uri}\")");
                        }
                    };
                };
                t.Start();
            };
            KeyDown += (_, e) =>
            {
                e.Handled = false;
                switch (e.Key)
                {
                    case Key.Escape:
                        MouseLock = !MouseLock;
                        e.Handled = true;
                        break;
                    case Key.F1:
                        WebView2.CoreWebView2.ExecuteScriptAsync("CreateIFrameInFront(\"https://google.com\")");
                        break;
                    case Key.F2:
                        WebView2.CoreWebView2.Navigate("edge://settings/privacy");
                        break;
                }
            };
        }
    }
}
