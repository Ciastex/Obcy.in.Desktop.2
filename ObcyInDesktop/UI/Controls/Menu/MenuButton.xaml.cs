using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace ObcyInDesktop.UI.Controls.Menu
{
    public partial class MenuButton
    {
        public Color IdleColor
        {
            get { return (Color)GetValue(IdleColorProperty); }
            set { SetValue(IdleColorProperty, value); }
        }

        public Color ActiveColor
        {
            get { return (Color)GetValue(ActiveColorProperty); }
            set { SetValue(ActiveColorProperty, value); }
        }

        public Color LabelColor
        {
            get { return (Color)GetValue(LabelColorProperty); }
            set { SetValue(LabelColorProperty, value); }
        }

        public ContextMenu Menu
        {
            get { return (ContextMenu)GetValue(MenuProperty); }
            set { SetValue(MenuProperty, value); }
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public MenuButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IdleColorProperty = DependencyProperty.Register(
            "IdleColor", typeof(Color), typeof(MenuButton)
        );

        public static readonly DependencyProperty ActiveColorProperty = DependencyProperty.Register(
            "ActiveColor", typeof(Color), typeof(MenuButton)
        );

        public static readonly DependencyProperty LabelColorProperty = DependencyProperty.Register(
            "LabelColor", typeof(Color), typeof(MenuButton)
        );

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
            "Label", typeof(string), typeof(MenuButton)
        );

        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(
            "Menu", typeof(ContextMenu), typeof(MenuButton)
        );

        public static readonly DependencyProperty IsMenuOpenProperty = DependencyProperty.Register(
            "IsMenuOpen", typeof(bool), typeof(MenuButton)
        );

        public void MenuButton_OnMouseDown(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Menu != null)
            {
                Menu.PlacementTarget = this;
                Menu.Placement = PlacementMode.Bottom;

                Menu.IsOpen = true;
            }
        }
    }
}
