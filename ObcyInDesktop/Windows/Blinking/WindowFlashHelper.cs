using System;
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

        private void InitializeHandle()
        {
            if (_mainWindowHWnd == IntPtr.Zero)
            {
                var mainWindow = _app.MainWindow;
                _mainWindowHWnd = new WindowInteropHelper(mainWindow).Handle;
            }
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

        private const uint FlashwAll = 3;
        private const uint FlashwTimerNoFg = 12;

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

        private static void Flash(IntPtr hwnd, uint count)
        {
            var fi = CreateFlashInfoStruct(hwnd, FlashwAll | FlashwTimerNoFg, count, 0);
            FlashWindowEx(ref fi);
        }
    }
}

