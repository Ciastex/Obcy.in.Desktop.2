using System.Windows.Media;

namespace ObcyInDesktop.UI
{
    static class Colors
    {
        public static Color WindowBackground { get; set; }
        public static Color WindowActiveBorder { get; set; }
        public static Color WindowActiveGlow { get; set; }
        public static Color WindowInactiveBorder { get; set; }
        public static Color WindowInactiveGlow { get; set; }
        public static Color WindowSeparator { get; set; }

        public static Color CaptionButtonIdle { get; set; }
        public static Color CaptionButtonHover { get; set; }

        public static Color PanelHideButtonIdle { get; set; }
        public static Color PanelHideButtonHover { get; set; }

        public static Color MainMenuButtonIdle { get; set; }
        public static Color MainMenuButtonHover { get; set; }

        public static Color IncomingMessageBackground { get; set; }
        public static Color IncomingMessageBorder { get; set; }
        public static Color OutgoingMessageBackground { get; set; }
        public static Color OutgoingMessageBorder { get; set; }
        public static Color TopicMessageBorder { get; set; }
        public static Color TopicMessageBackground { get; set; }

        public static Color ToggleButtonIdle { get; set; }
        public static Color ToggleButtonHover { get; set; }
        public static Color ToggleButtonChecked { get; set; }

        public static Color GroupBoxBorder { get; set; }

        public static Color TextBoxBackground { get; set; }
        public static Color TextBoxForeground { get; set; }
        public static Color TextBoxBorder { get; set; }
        public static Color TextBoxDisabled { get; set; }
        public static Color TextBoxReadOnly { get; set; }

        public static Color CheckBoxBorderIdle { get; set; }
        public static Color CheckBoxBorderHover { get; set; }
        public static Color CheckBoxFill { get; set; }

        public static Color ControlIdleBorder { get; set; }
        public static Color ControlHoverBorder { get; set; }
        public static Color ControlDisabled { get; set; }

        public static Color ToolTipBackground { get; set; }
        public static Color ToolTipBorder { get; set; }

        public static Color ButtonIdleBackground { get; set; }
        public static Color ButtonIdleBorder { get; set; }
        public static Color ButtonHoverBackground { get; set; }
        public static Color ButtonHoverBorder { get; set; }
        public static Color ButtonClickBackground { get; set; }
        public static Color ButtonClickBorder { get; set; }

        public static Color ScrollbarIdleForeground { get; set; }
        public static Color ScrollbarHoverForeground { get; set; }
        public static Color ScrollbarClickForeground { get; set; }

        public static Color WindowFont { get; set; }
        public static Color AccentedFont { get; set; }
        public static Color ActiveFont { get; set; }
        public static Color MildFont { get; set; }
        public static Color MenuHeaderFont { get; set; }
        public static Color MenuItemFont { get; set; }
        public static Color TextBlockFont { get; set; }
        public static Color ButtonFont { get; set; }
        public static Color ToolTipFont { get; set; }
        public static Color ListBoxItemInactiveFont { get; set; }
        public static Color ListBoxItemActiveFont { get; set; }

        public static Color MenuHighlight { get; set; }
        public static Color MenuBackground { get; set; }
        public static Color MenuBorder { get; set; }

        public static Color ComboBoxDropdownArrow { get; set; }

        public static Color ListBoxBorder { get; set; }
        public static Color ListBoxBackground { get; set; }
        public static Color ListBoxItemHoverBackground { get; set; }
        public static Color ListBoxItemActiveSelectionBackground { get; set; }
        public static Color ListBoxItemInactiveSelectionBackground { get; set; }

        public static SolidColorBrush WindowBackgroundBrush => new SolidColorBrush(WindowBackground);
        public static SolidColorBrush WindowActiveBorderBrush => new SolidColorBrush(WindowActiveBorder);
        public static SolidColorBrush WindowInactiveBorderBrush => new SolidColorBrush(WindowInactiveBorder);
        public static SolidColorBrush WindowActiveGlowBrush => new SolidColorBrush(WindowActiveGlow);
        public static SolidColorBrush WindowInactiveGlowBrush => new SolidColorBrush(WindowInactiveGlow);
        public static SolidColorBrush WindowSeparatorBrush => new SolidColorBrush(WindowSeparator);

        public static SolidColorBrush CaptionButtonIdleBrush => new SolidColorBrush(CaptionButtonIdle);
        public static SolidColorBrush CaptionButtonHoverBrush => new SolidColorBrush(CaptionButtonHover);

        public static SolidColorBrush PanelHideButtonIdleBrush => new SolidColorBrush(PanelHideButtonIdle);
        public static SolidColorBrush PanelHideButtonHoverBrush => new SolidColorBrush(PanelHideButtonHover);

        public static SolidColorBrush MainMenuButtonIdleBrush => new SolidColorBrush(MainMenuButtonIdle);
        public static SolidColorBrush MainMenuButtonHoverBrush => new SolidColorBrush(MainMenuButtonHover);

        public static SolidColorBrush IncomingMessageBackgroundBrush => new SolidColorBrush(IncomingMessageBackground);
        public static SolidColorBrush IncomingMessageBorderBrush => new SolidColorBrush(IncomingMessageBorder);
        public static SolidColorBrush OutgoingMessageBackgroundBrush => new SolidColorBrush(OutgoingMessageBackground);
        public static SolidColorBrush OutgoingMessageBorderBrush => new SolidColorBrush(OutgoingMessageBorder);
        public static SolidColorBrush TopicMessageBackgroundBrush => new SolidColorBrush(TopicMessageBackground);
        public static SolidColorBrush TopicMessageBorderBrush => new SolidColorBrush(TopicMessageBorder);

        public static SolidColorBrush ToggleButtonIdleBrush => new SolidColorBrush(ToggleButtonIdle);
        public static SolidColorBrush ToggleButtonHoverBrush => new SolidColorBrush(ToggleButtonHover);
        public static SolidColorBrush ToggleButtonCheckedBrush => new SolidColorBrush(ToggleButtonChecked);

        public static SolidColorBrush GroupBoxBorderBrush => new SolidColorBrush(GroupBoxBorder);

        public static SolidColorBrush TextBoxBackgroundBrush => new SolidColorBrush(TextBoxBackground);
        public static SolidColorBrush TextBoxForegroundBrush => new SolidColorBrush(TextBoxForeground);
        public static SolidColorBrush TextBoxBorderBrush => new SolidColorBrush(TextBoxBorder);
        public static SolidColorBrush TextBoxDisabledBrush => new SolidColorBrush(TextBoxDisabled);
        public static SolidColorBrush TextBoxReadOnlyBrush => new SolidColorBrush(TextBoxReadOnly);
 
        public static SolidColorBrush CheckBoxBorderIdleBrush => new SolidColorBrush(CheckBoxBorderIdle);
        public static SolidColorBrush CheckBoxBorderHoverBrush => new SolidColorBrush(CheckBoxBorderHover);
        public static SolidColorBrush CheckBoxFillBrush => new SolidColorBrush(CheckBoxFill);

        public static SolidColorBrush ControlIdleBorderBrush => new SolidColorBrush(ControlIdleBorder);
        public static SolidColorBrush ControlHoverBorderBrush => new SolidColorBrush(ControlHoverBorder);
        public static SolidColorBrush ControlDisabledBrush => new SolidColorBrush(ControlDisabled);

        public static SolidColorBrush ToolTipBackgroundBrush => new SolidColorBrush(ToolTipBackground);
        public static SolidColorBrush ToolTipBorderBrush => new SolidColorBrush(ToolTipBorder);

        public static SolidColorBrush ButtonIdleBackgroundBrush => new SolidColorBrush(ButtonIdleBackground);
        public static SolidColorBrush ButtonIdleBorderBrush => new SolidColorBrush(ButtonIdleBorder);
        public static SolidColorBrush ButtonHoverBackgroundBrush => new SolidColorBrush(ButtonHoverBackground);
        public static SolidColorBrush ButtonHoverBorderBrush => new SolidColorBrush(ButtonHoverBorder);
        public static SolidColorBrush ButtonClickBackgroundBrush => new SolidColorBrush(ButtonClickBackground);
        public static SolidColorBrush ButtonClickBorderBrush => new SolidColorBrush(ButtonClickBorder);

        public static SolidColorBrush ScrollbarIdleForegroundBrush => new SolidColorBrush(ScrollbarIdleForeground);
        public static SolidColorBrush ScrollbarHoverForegroundBrush => new SolidColorBrush(ScrollbarHoverForeground);
        public static SolidColorBrush ScrollbarClickForegroundBrush => new SolidColorBrush(ScrollbarClickForeground);

        public static SolidColorBrush WindowFontBrush => new SolidColorBrush(WindowFont);
        public static SolidColorBrush AccentedFontBrush => new SolidColorBrush(AccentedFont);
        public static SolidColorBrush ActiveFontBrush => new SolidColorBrush(ActiveFont);
        public static SolidColorBrush MildFontBrush => new SolidColorBrush(MildFont);
        public static SolidColorBrush MenuHeaderFontBrush => new SolidColorBrush(MenuHeaderFont);
        public static SolidColorBrush MenuItemFontBrush => new SolidColorBrush(MenuItemFont);
        public static SolidColorBrush TextBlockFontBrush => new SolidColorBrush(TextBlockFont);
        public static SolidColorBrush ButtonFontBrush => new SolidColorBrush(ButtonFont);
        public static SolidColorBrush ToolTipFontBrush => new SolidColorBrush(ToolTipFont);
        public static SolidColorBrush ListBoxItemInactiveFontBrush => new SolidColorBrush(ListBoxItemInactiveFont);
        public static SolidColorBrush ListBoxItemActiveFontBrush => new SolidColorBrush(ListBoxItemActiveFont);

        public static SolidColorBrush MenuHighlightBrush => new SolidColorBrush(MenuHighlight);
        public static SolidColorBrush MenuBackgroundBrush => new SolidColorBrush(MenuBackground);
        public static SolidColorBrush MenuBorderBrush => new SolidColorBrush(MenuBorder);

        public static SolidColorBrush ComboBoxDropdownArrowBrush => new SolidColorBrush(ComboBoxDropdownArrow);

        public static SolidColorBrush ListBoxBorderBrush => new SolidColorBrush(ListBoxBorder);
        public static SolidColorBrush ListBoxBackgroundBrush => new SolidColorBrush(ListBoxBackground);
        public static SolidColorBrush ListBoxItemActiveSelectionBackgroundBrush => new SolidColorBrush(ListBoxItemActiveSelectionBackground);
        public static SolidColorBrush ListBoxItemInactiveSelectionBackgroundBrush => new SolidColorBrush(ListBoxItemInactiveSelectionBackground);
        public static SolidColorBrush ListBoxItemHoverBackgroundBrush => new SolidColorBrush(ListBoxItemHoverBackground);

        static Colors()
        {
            WindowBackground = HexColor("FFFFFF");
            WindowActiveBorder = HexColor("007BDA");
            WindowActiveGlow = HexColor("007BDA");
            WindowInactiveGlow = HexColor("000000");
            WindowSeparator = HexColor("DADBDC");

            CaptionButtonIdle = HexColor("000000");
            CaptionButtonHover = HexColor("777777");
            PanelHideButtonIdle = HexColor("A0A0A0");
            PanelHideButtonHover = HexColor("707070");

            MainMenuButtonIdle = HexColor("007BDA");
            MainMenuButtonHover = HexColor("1979CA");

            IncomingMessageBackground = HexColor("4C8CCB");
            IncomingMessageBorder = HexColor("4C8CCB");
            OutgoingMessageBackground = HexColor("3F9BF6");
            OutgoingMessageBorder = HexColor("3F9BF6");
            TopicMessageBackground = HexColor("4C8CCB");
            TopicMessageBorder = HexColor("4C8CCB");

            ToggleButtonIdle = HexColor("FFFFFF");
            ToggleButtonHover = HexColor("F0F0F0");
            ToggleButtonChecked = HexColor("E5E5E5");

            GroupBoxBorder = HexColor("218BE4");

            TextBoxBackground = HexColor("FFFFFF");
            TextBoxForeground = HexColor("000000");
            TextBoxBorder = HexColor("4C8CCB");
            TextBoxDisabled = HexColor("EDEDED");
            TextBoxReadOnly = HexColor("F5F5F5");

            CheckBoxBorderIdle = HexColor("AFAFAF");
            CheckBoxBorderHover = HexColor("4C8CCB");
            CheckBoxFill = HexColor("4C8CCB");

            ControlIdleBorder = HexColor("AFAFAF");
            ControlHoverBorder = HexColor("4C8CCB");
            ControlDisabled = HexColor("DDDDDD");

            ToolTipBorder = HexColor("F0F0F0");
            ToolTipBackground = HexColor("FFFFFF");

            ButtonIdleBackground = HexColor("FFFFFF");
            ButtonIdleBorder = HexColor("1979CA");
            ButtonHoverBackground = HexColor("1979CA");
            ButtonHoverBorder = HexColor("1979CA");
            ButtonClickBackground = HexColor("588FC5");
            ButtonClickBorder = HexColor("1979CA");

            ScrollbarIdleForeground = HexColor("BFBFBF");
            ScrollbarHoverForeground = HexColor("ADADAD");
            ScrollbarClickForeground = HexColor("666666");

            WindowFont = HexColor("000000");
            AccentedFont = HexColor("1979CA");
            ActiveFont = HexColor("FFFFFF");
            MildFont = HexColor("ABABAB");
            MenuHeaderFont = HexColor("000000");
            MenuItemFont = HexColor("000000");
            TextBlockFont = HexColor("000000");
            ButtonFont = HexColor("1979CA");
            ToolTipFont = HexColor("000000");
            ListBoxItemInactiveFont = HexColor("000000");
            ListBoxItemActiveFont = HexColor("FFFFFF");

            MenuHighlight = HexColor("DEDEDE");
            MenuBackground = HexColor("FFFFFF");
            MenuBorder = HexColor("A0A0A0");

            ComboBoxDropdownArrow = HexColor("666666");

            ListBoxBorder = HexColor("DADBDC");
            ListBoxBackground = HexColor("FFFFFF");
            ListBoxItemActiveSelectionBackground = HexColor("1979CA");
            ListBoxItemInactiveSelectionBackground = HexColor("1979CA");
            ListBoxItemHoverBackground = HexColor("DEDEDE");
        }

        public static Color ToColor(this string str)
        {
            return HexColor(str);
        }

        private static Color HexColor(string hexString)
        {
            var col = ColorConverter.ConvertFromString($"#{hexString}");

            if (col != null)
                return (Color)col;

            return new Color { A = 255, R = 0, G = 0, B = 0 };
        }
    }
}
