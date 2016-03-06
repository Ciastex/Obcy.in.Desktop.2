using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ObcyInDesktop.UI.Commands
{
    public class WindowCommands
    {
        private static double _prevX;
        private static double _prevY;
        private static double _prevWidth;
        private static double _prevHeight;

        private static bool _isMaximized;

        public static readonly ICommand CloseCommand = new RelayCommand(o =>
        {
            var parent = o as Window;
            Debug.Assert(parent != null, "parent != null");

            parent.Close();
        });

        public static readonly ICommand MaximizeCommand = new RelayCommand(o =>
        {
            var parent = o as Window;
            Debug.Assert(parent != null, "parent != null");

            if (_isMaximized)
            {
                var windowBorder = (Border)parent.FindName("WindowBorder");
                if (windowBorder != null)
                {
                    windowBorder.Margin = new Thickness(5);
                    windowBorder.BorderThickness = new Thickness(1);
                }

                var captionBorder = (Border)parent.Template.FindName("CaptionBar", parent);
                if (captionBorder != null)
                {
                    captionBorder.Margin = new Thickness(174, 6, 6, 0);
                }

                parent.Left = _prevX;
                parent.Top = _prevY;
                _prevX = 0;
                _prevY = 0;

                parent.Width = _prevWidth;
                parent.Height = _prevHeight;

                parent.ResizeMode = ResizeMode.CanResize;

                _isMaximized = false;
            }
            else
            {
                var windowBorder = (Border)parent.FindName("WindowBorder");
                if (windowBorder != null)
                {
                    windowBorder.Margin = new Thickness(0);
                    windowBorder.BorderThickness = new Thickness(0);
                }

                var captionBorder = (Border)parent.Template.FindName("CaptionBar", parent);
                if (captionBorder != null)
                {
                    captionBorder.Margin = new Thickness(174, 0, 2, 0);
                }

                _prevX = parent.Left;
                _prevY = parent.Top;
                _prevWidth = parent.Width;
                _prevHeight = parent.Height;

                parent.Left = 0;
                parent.Top = 0;

                parent.Width = SystemParameters.WorkArea.Width;
                parent.Height = SystemParameters.WorkArea.Height;

                parent.ResizeMode = ResizeMode.CanMinimize;

                _isMaximized = true;
            }
        });

        public static readonly ICommand MinimizeCommand = new RelayCommand(o =>
        {
            var parent = o as Window;
            Debug.Assert(parent != null, "parent != null");

            parent.WindowState = WindowState.Minimized;
        });

        public static readonly ICommand DragWindow = new RelayCommand(o =>
        {
            var parent = o as Window;
            Debug.Assert(parent != null, "parent != null");

            if (!_isMaximized)
            {
                parent.DragMove();
            }
        });
    }
}
