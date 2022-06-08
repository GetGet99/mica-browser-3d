using Handler3D;
using Windows.UI.Core;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Media3D;
using Windows.System;
using System.Windows.Input;
using PInvoke;
using Microsoft.UI.Xaml.Media.Animation;
using System.Diagnostics;
using System;
using System.Drawing;
using System.Threading;

namespace Mica_Browser_3D_WinUI_3;

public sealed partial class MainWindow : Window
{

    bool _MouseLock;
    Rectangle WindowRect
    {
        get
        {
            User32.GetWindowRect(Handle, out var rect);
            return new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        }
    }
    Size Size => WindowRect.Size;
    bool MouseLock
    {
        get => _MouseLock;
        set
        {
            if (value)
            {
                var Rect = WindowRect;
                var centerX = Rect.Width / 2;
                var centerY = Rect.Height / 2;
                centerX += Rect.Left;
                centerY += Rect.Top;
                User32.SetCursorPos(centerX, centerY);
            }
            _MouseLock = value;
        }
    }
    IntPtr Handle => WinRT.Interop.WindowNative.GetWindowHandle(this);
    public MainWindow()
    {
        //var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
        //var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
        InitializeComponent();

        ExtendsContentIntoTitleBar = true;
        SetTitleBar(TitleBarElement);
        MicaHandler.InitMica(this);
        bool FirstRun = true;
        Activated += delegate
        {
            if (!FirstRun) return;

            //var cam = new PersepctiveCamera();
            //var transform = cam.LocalTransform;
            //transform.Position = new Vector3
            //{
            //    X = 0,
            //    Y = 0,
            //    Z = -1
            //};
            //cam.LocalTransform = transform;


        };
        const double MoveSpeed = 3, MaxSpeed = 3, Friction = 0.91, MouseSensitiveX = 0.3, MouseSensitiveY = 0.1;
        Vector3 Velocity = new(), MoveInput = new();
        double dX = 0, dY = 0;
        var obj = new Object3D();
        var transform = obj.LocalTransform;
        var compositeTransform3D = new CompositeTransform3D();
        myButton.Transform3D = compositeTransform3D;
        var cameraMatrix = Matrix4x4.Compose(Vector3.Zero, new DegreeEuler(0, 0, 0), Vector3.One);
        var buttonMatrix = Matrix4x4.Compose(new Vector3(0, 0, 10), new DegreeEuler(0, 0, 0), Vector3.One);
        //myButton.Content = 0;
        FirstRun = true;
        bool EscapeDown = false;
        var InputTimer = DispatcherQueue.CreateTimer();
        var RenderTimer = DispatcherQueue.CreateTimer();
        InputTimer.Interval = TimeSpan.FromMilliseconds(1000 / 60); // 60 FPS
        RenderTimer.Interval = TimeSpan.FromMilliseconds(1000 / 144); // 144 FPS
        InputTimer.Tick += delegate
        {
            try
            {
                Vector3 LMoveInput = new();
                if (IsKeyDown(User32.VirtualKey.VK_W)) LMoveInput.Z += 1;
                if (IsKeyDown(User32.VirtualKey.VK_A)) LMoveInput.X += 1;
                if (IsKeyDown(User32.VirtualKey.VK_S)) LMoveInput.Z -= 1;
                if (IsKeyDown(User32.VirtualKey.VK_D)) LMoveInput.X -= 1;
                if (IsKeyDown(User32.VirtualKey.VK_SPACE)) LMoveInput.Y += 1;
                if (IsKeyDown(User32.VirtualKey.VK_LSHIFT) || IsKeyDown(User32.VirtualKey.VK_RSHIFT)) LMoveInput.Y -= 1;
                if (IsKeyDown(User32.VirtualKey.VK_ESCAPE))
                {
                    if (!EscapeDown)
                    {
                        EscapeDown = true;
                        MouseLock = !MouseLock;
                    }
                }
                else
                    if (EscapeDown) EscapeDown = false;
                MoveInput = LMoveInput;
                if (MouseLock)
                {
                    var (pos, quad, scale) = cameraMatrix.Decompose();
                    User32.GetCursorPos(out var cursorPos);
                    var Rect = WindowRect;
                    var centerX = Rect.Width / 2;
                    var centerY = Rect.Height / 2;
                    centerX += Rect.Left;
                    centerY += Rect.Top;
                    dX = centerX - cursorPos.x;
                    dY = centerY - cursorPos.y;
                    User32.SetCursorPos(centerX, centerY);
                }
            } catch
            {

            }
        };
        InputTimer.IsRepeating = true;
        
        Thread t = new((ThreadStart)delegate
        {
            while (true)
            {
                const int ms = 1000 / 200;
                Thread.Sleep(ms);
                if (!InputTimer.IsRunning) InputTimer.Start();
                if (!RenderTimer.IsRunning) RenderTimer.Start();
                if (MoveInput != Vector3.Zero)
                {
                    Velocity += MoveInput * MoveSpeed;
                }

                bool VelocityUpdate = Velocity != Vector3.Zero;
                bool MouseUpdate = dX > 0 || dY > 0;
                if (VelocityUpdate || MouseUpdate)
                {
                    var (pos, quad, scale) = cameraMatrix.Decompose();
                    var _euler = quad.ToEuler();
                    var rotMouseY = _euler.Y;
                    bool UpdateAll = false;
                    if (VelocityUpdate)
                    {
                        Velocity = Velocity.Clamp(-MaxSpeed, MaxSpeed);
                        Velocity *= Friction;
                        pos.Z += Velocity.Z * Math.Cos(rotMouseY);
                        pos.X += Velocity.Z * Math.Sin(rotMouseY);
                        pos.Z -= Velocity.X * Math.Sin(rotMouseY);
                        pos.X += Velocity.X * Math.Cos(rotMouseY);
                        pos.Y += Velocity.Y;
                    }
                    if (MouseUpdate)
                    {
                        var degeuler = _euler.ToDegree();
                        var rotX = degeuler.X + dX * MouseSensitiveX;
                        var rotY = Math.Clamp(degeuler.Y - dY * MouseSensitiveY, -90, 90);
                        //degeuler.X = rotY;
                        degeuler.Y = rotX;
                        degeuler.Z = 0;
                        quad = degeuler.ToRadian().ToQuaternion();
                        dX = 0;
                        dY = 0;
                        UpdateAll = true;
                    }
                    if (UpdateAll)
                        cameraMatrix = Matrix4x4.Compose(pos, quad, scale);
                    else
                        cameraMatrix.Position = pos;
                }
            }
        });
        t.Start();
        
        RenderTimer.Tick += delegate
        {
            var screen = buttonMatrix.RelativeTo(cameraMatrix);
            screen.SetCompositeTransform3D(compositeTransform3D);
            var euler = cameraMatrix.DegreeEuler;
            RotX.Text = $"{nameof(RotX)}: {euler.X}";
            RotY.Text = $"{nameof(RotY)}: {euler.Y}";
            RotZ.Text = $"{nameof(RotZ)}: {euler.Z}";
        };
        RenderTimer.IsRepeating = true;
        InputTimer.Start();
        RenderTimer.Start();
    }
    static bool IsKeyDown(User32.VirtualKey Key) => (User32.GetKeyState((int)Key) & (1 << 15)) != 0;
}

