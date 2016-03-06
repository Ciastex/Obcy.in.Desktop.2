using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ObcyInDesktop.Windows.Blinking
{
    public class WindowFlashHelper
    {
        private IntPtr _mainWindowHWnd;
        private readonly Application _app;

        public WindowFlashHelper(Application app)
        {
            _app = app;
        }

        public void FlashApplicationWindow()
        {
            InitializeHandle();
            Flash(_mainWindowHWnd, 5);
        }

        public void StopFlashing()
        {
            var fi = CreateFlashInfoStruct(_mainWindowHWnd, FlashwStop, uint.MaxValue, 0);
            FlashWindowEx(ref fi);
        }

        private void InitializeHandle()
        {
            if (_mainWindowHWnd == IntPtr.Zero)
            {
                var mainWindow = _app.MainWindow;
                _mainWindowHWnd = new WindowInteropHelper(mainWindow).Handle;
            }
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        private static bool IsActive(Window wnd)
        {
            if (wnd == null) return false;
            return GetForegroundWindow() == new WindowInteropHelper(wnd).Handle;
        }

        public static bool IsApplicationActive()
        {
            return Application.Current.Windows.OfType<Window>().Any(IsActive);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FlashWinfo pwfi);

        [StructLayout(LayoutKind.Sequential)]

        private struct FlashWinfo
        {
            public uint cbSize;
            public IntPtr hwnd;
            public uint dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }

        public const uint FlashwStop = 0;
        public const uint FlashwCaption = 1;
        public const uint FlashwTray = 2;
        public const uint FlashwAll = 3;
        public const uint FlashwTimer = 4;
        public const uint FlashwTimerNoFg = 12;

        public static bool Flash(IntPtr hwnd)
        {
            FlashWinfo fi = CreateFlashInfoStruct(hwnd, FlashwAll | FlashwTimerNoFg, uint.MaxValue, 0);
            return FlashWindowEx(ref fi);
        }

        private static FlashWinfo CreateFlashInfoStruct(IntPtr handle, uint flags, uint count, uint timeout)
        {
            var fi = new FlashWinfo();
            fi.cbSize = Convert.ToUInt32(Marshal.SizeOf(fi));
            fi.hwnd = handle;
            fi.dwFlags = flags;
            fi.uCount = count;
            fi.dwTimeout = timeout;
            return fi;
        }

        public static bool Flash(IntPtr hwnd, uint count)
        {
            FlashWinfo fi = CreateFlashInfoStruct(hwnd, FlashwAll | FlashwTimerNoFg, count, 0);
            return FlashWindowEx(ref fi);
        }
    }
}

