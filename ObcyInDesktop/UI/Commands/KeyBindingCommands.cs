using System.Windows.Input;

namespace ObcyInDesktop.UI.Commands
{
    public static class KeyBindingCommands
    {
        public static RoutedCommand ClearLogCommand = new RoutedCommand();
        public static RoutedCommand SwitchStrangerCommand = new RoutedCommand();
        public static RoutedCommand FlagStrangerCommand = new RoutedCommand();
        public static RoutedCommand ToggleCopyViewCommand = new RoutedCommand();
        public static RoutedCommand ToggleScrollingCommand = new RoutedCommand();

        static KeyBindingCommands()
        {
            ClearLogCommand.InputGestures.Add(
                new KeyGesture(Key.L, ModifierKeys.Control)
            );

            SwitchStrangerCommand.InputGestures.Add(
                new KeyGesture(Key.Escape)
            );

            FlagStrangerCommand.InputGestures.Add(
                new KeyGesture(Key.F, ModifierKeys.Control)
            );

            ToggleCopyViewCommand.InputGestures.Add(
                new KeyGesture(Key.D, ModifierKeys.Control)
            );

            ToggleScrollingCommand.InputGestures.Add(
                new KeyGesture(Key.H, ModifierKeys.Control)
            );
        }
    }
}
