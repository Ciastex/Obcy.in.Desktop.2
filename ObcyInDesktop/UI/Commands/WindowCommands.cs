using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ObcyInDesktop.UI.Commands
{
    public class WindowCommands
    {
        private static double _prevX;
        private static double _prevY;
        private static double _prevWidth;
        private static double _prevHeight;

        private static Window _parent;
        private static Button _button;
        private static Border _captionBar;
        private static Border _menuContainer;
        private static Grid _leftPanelArea;
        private static Border _rightPanelArea;

        private static bool _isMaximized;

        private static readonly ThicknessAnimation MarginCollapseAnimation = new ThicknessAnimation(
            new Thickness(180, 6, 6, 0),
            new Thickness(6, 6, 6, 0),
            new Duration(new TimeSpan(0, 0, 0, 0, 200))
        );

        private static readonly ThicknessAnimation MarginIncreaseAnimation = new ThicknessAnimation(
            new Thickness(6, 6, 6, 0),
            new Thickness(180, 6, 6, 0),
            new Duration(new TimeSpan(0, 0, 0, 0, 200))
        );

        private static readonly ThicknessAnimation RightPanelMarginCollapseAnimation = new ThicknessAnimation(
            new Thickness(174, 25, 0, 0),
            new Thickness(0, 25, 0, 0),
            new Duration(new TimeSpan(0, 0, 0, 0, 200))
        );

        private static readonly ThicknessAnimation RightPanelMarginIncreaseAnimation = new ThicknessAnimation(
            new Thickness(0, 25, 0, 0),
            new Thickness(174, 25, 0, 0),
            new Duration(new TimeSpan(0, 0, 0, 0, 200))
        );

        private static readonly DoubleAnimation LeftPanelFadeOutAnimation = new DoubleAnimation(
            1, 0, new Duration(new TimeSpan(0, 0, 0, 0, 200))
        );

        private static readonly DoubleAnimation LeftPanelFadeInAnimation = new DoubleAnimation(
            0, 1, new Duration(new TimeSpan(0, 0, 0, 0, 200))
        );

        private static readonly DoubleAnimation MenuContainerFadeOutAnimation = new DoubleAnimation(
            1, 0, new Duration(new TimeSpan(0, 0, 0, 0, 200))
        );

        private static readonly DoubleAnimation MenuContainerFadeInAnimation = new DoubleAnimation(
            0, 1, new Duration(new TimeSpan(0, 0, 0, 0, 200))
        );

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

            switch (_isMaximized)
            {
                case true:
                    parent.Left = _prevX;
                    parent.Top = _prevY;
                    _prevX = 0;
                    _prevY = 0;

                    parent.Width = _prevWidth;
                    parent.Height = _prevHeight;

                    _isMaximized = false;
                    break;
                default:
                    _prevX = parent.Left;
                    _prevY = parent.Top;
                    _prevWidth = parent.Width;
                    _prevHeight = parent.Height;

                    parent.Left = 0;
                    parent.Top = 0;

                    parent.Width = SystemParameters.WorkArea.Width;
                    parent.Height = SystemParameters.WorkArea.Height;

                    _isMaximized = true;
                    break;
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

            parent.DragMove();
        });

        public static readonly ICommand TogglePanel = new RelayCommand(o =>
        {
            _parent = o as Window;
            Debug.Assert(_parent != null, "parentWindow != null");

            _leftPanelArea = _parent.FindName("LeftPanelArea") as Grid;
            Debug.Assert(_leftPanelArea != null, "leftPanelArea != null");

            _rightPanelArea = _parent.FindName("RightPanelArea") as Border;
            Debug.Assert(_rightPanelArea != null, "rightPanelArea != null");

            _menuContainer = _parent.FindName("MenuContainer") as Border;
            Debug.Assert(_menuContainer != null, "menuContainer != null");

            var captionBarDependencyObject = _parent.Template.FindName("CaptionBar", _parent) as DependencyObject;
            Debug.Assert(captionBarDependencyObject != null, "captionBarDependencyObject != null");

            var captionBarParentGrid = VisualTreeHelper.GetChild(captionBarDependencyObject, 0) as Grid;
            Debug.Assert(captionBarParentGrid != null, "captionBarParentGrid != null");

            _captionBar = (captionBarParentGrid).FindName("CaptionBar") as Border;
            Debug.Assert(_captionBar != null, "captionBar != null");

            _button = captionBarParentGrid.FindName("HidePanelButton") as Button;
            Debug.Assert(_button != null, "button != null");

            if (_captionBar.Margin == new Thickness(6, 6, 6, 0))
            {
                MarginIncreaseAnimation.Completed += marginIncreaseAnimation_Completed;
                RightPanelMarginIncreaseAnimation.Completed += rightPanelMarginIncreaseAnimation_Completed;

                _captionBar.BeginAnimation(FrameworkElement.MarginProperty, MarginIncreaseAnimation);
                _rightPanelArea.BeginAnimation(FrameworkElement.MarginProperty, RightPanelMarginIncreaseAnimation);
            }
            else
            {
                LeftPanelFadeOutAnimation.Completed += leftPanelFadeOutAnimation_Completed;
                MenuContainerFadeOutAnimation.Completed += menuContainerFadeOutAnimation_Completed;

                _leftPanelArea.BeginAnimation(UIElement.OpacityProperty, LeftPanelFadeOutAnimation);
                _menuContainer.BeginAnimation(UIElement.OpacityProperty, MenuContainerFadeOutAnimation);
            }
        });

        static void rightPanelMarginIncreaseAnimation_Completed(object sender, EventArgs e)
        {
            RightPanelMarginIncreaseAnimation.Completed -= rightPanelMarginIncreaseAnimation_Completed;
        }

        static void menuContainerFadeOutAnimation_Completed(object sender, EventArgs e)
        {
            _menuContainer.Visibility = Visibility.Collapsed;
            MenuContainerFadeOutAnimation.Completed -= menuContainerFadeOutAnimation_Completed;
        }

        static void leftPanelFadeOutAnimation_Completed(object sender, EventArgs e)
        {
            MarginCollapseAnimation.Completed += marginCollapseAnimation_Completed;
            RightPanelMarginCollapseAnimation.Completed += rightPanelMarginCollapseAnimation_Completed;

            _captionBar.BeginAnimation(FrameworkElement.MarginProperty, MarginCollapseAnimation);
            _rightPanelArea.BeginAnimation(FrameworkElement.MarginProperty, RightPanelMarginCollapseAnimation);

            LeftPanelFadeOutAnimation.Completed -= leftPanelFadeOutAnimation_Completed;
        }

        static void rightPanelMarginCollapseAnimation_Completed(object sender, EventArgs e)
        {
            RightPanelMarginCollapseAnimation.Completed -= rightPanelMarginCollapseAnimation_Completed;
        }

        private static void marginIncreaseAnimation_Completed(object sender, EventArgs e)
        {
            _button.LayoutTransform = new RotateTransform(0);

            _leftPanelArea.Width = 174;
            _leftPanelArea.Visibility = Visibility.Visible;

            MenuContainerFadeInAnimation.Completed += menuContainerFadeInAnimation_Completed;
            LeftPanelFadeInAnimation.Completed += leftPanelFadeInAnimation_Completed;

            _menuContainer.Visibility = Visibility.Visible;

            _menuContainer.BeginAnimation(UIElement.OpacityProperty, MenuContainerFadeInAnimation);
            _leftPanelArea.BeginAnimation(UIElement.OpacityProperty, LeftPanelFadeInAnimation);

            MarginIncreaseAnimation.Completed -= marginIncreaseAnimation_Completed;
        }

        static void leftPanelFadeInAnimation_Completed(object sender, EventArgs e)
        {
            LeftPanelFadeInAnimation.Completed -= leftPanelFadeInAnimation_Completed;
        }

        static void menuContainerFadeInAnimation_Completed(object sender, EventArgs e)
        {
            MenuContainerFadeInAnimation.Completed -= menuContainerFadeInAnimation_Completed;
        }

        private static void marginCollapseAnimation_Completed(object sender, EventArgs eventArgs)
        {
            _button.LayoutTransform = new RotateTransform(180);

            _leftPanelArea.Width = 0;
            _leftPanelArea.Visibility = Visibility.Collapsed;

            MarginCollapseAnimation.Completed -= marginCollapseAnimation_Completed;
        }
    }
}
