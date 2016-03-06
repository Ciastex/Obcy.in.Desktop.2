using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using ObcyInDesktop.Filesystem;
using ObcyInDesktop.Settings;

namespace ObcyInDesktop.UI
{
    public class ColorSchemeLoader
    {
        public Dictionary<string, Dictionary<string, string>> ColorSchemes { get; }

        public bool CurrentSchemeUsesLightIcons => ColorSchemes[ColorSchemeID]["UsesLightIconTheme"] == "true";
        public bool CurrentSchemeUsesDarkIcons => ColorSchemes[ColorSchemeID]["UsesDarkIconTheme"] == "true";

        private string ColorSchemeID { get; }

        public ColorSchemeLoader()
        {
            ColorSchemes = new Dictionary<string, Dictionary<string, string>>();
            ColorSchemeID = SettingsSelector.GetConfigurationValue<string>("ColorScheme");
        }

        public void LoadColorSchemes()
        {
            var fileList = Directory.GetFiles(DirectoryGuard.ColorSchemesDirectory, "*.xml");

            foreach (var s in fileList)
            {
                var xDoc = XDocument.Load(s);

                if (xDoc.Root == null)
                {
                    continue;
                }

                var colors = xDoc.XPathSelectElements("/ColorScheme/Color");

                var lightColorScheme = xDoc.Root.XPathSelectElement("UsesLightIconTheme");
                var darkColorScheme = xDoc.Root.XPathSelectElement("UsesDarkIconTheme");

                var dict = colors.ToDictionary(color => color.Attribute("Key").Value, color => color.Attribute("Value").Value);

                if (darkColorScheme != null)
                {
                    lightColorScheme = null; // to prevent using both at the same time
                    dict.Add("UsesDarkIconTheme", "true");
                }
                else
                {
                    dict.Add("UsesDarkIconTheme", "false");
                }

                dict.Add("UsesLightIconTheme", 
                         lightColorScheme != null ? "true" : "false");

                if (xDoc.Root != null)
                    ColorSchemes.Add(xDoc.Root.Attribute("Name").Value, dict);
            }

            ApplyColors();
        }

        public void ApplyColors()
        {
            if (!ColorSchemes.ContainsKey(ColorSchemeID))
            {
                return;
            }

            ApplyWindowStyle();
            ApplyMenuStyle();
            ApplyMessageControlStyle();
            ApplyTextBoxStyle();
            ApplyCheckBoxStyle();
            ApplyControlStyle();
            ApplyButtonStyle();
            ApplyScrollbarStyle();
            ApplyFontStyle();
        }

        private void ApplyTextBoxStyle()
        {
            Colors.TextBoxBackground = ColorSchemes[ColorSchemeID]["TextBoxBackground"].ToColor();
            Colors.TextBoxForeground = ColorSchemes[ColorSchemeID]["TextBoxForeground"].ToColor();
            Colors.TextBoxBorder = ColorSchemes[ColorSchemeID]["TextBoxBorder"].ToColor();
            Colors.TextBoxDisabled = ColorSchemes[ColorSchemeID]["TextBoxDisabled"].ToColor();
            Colors.TextBoxReadOnly = ColorSchemes[ColorSchemeID]["TextBoxReadOnly"].ToColor();
        }

        private void ApplyFontStyle()
        {
            Colors.WindowFont = ColorSchemes[ColorSchemeID]["WindowFont"].ToColor();
            Colors.AccentedFont = ColorSchemes[ColorSchemeID]["AccentedFont"].ToColor();
            Colors.ActiveFont = ColorSchemes[ColorSchemeID]["ActiveFont"].ToColor();
            Colors.MildFont = ColorSchemes[ColorSchemeID]["MildFont"].ToColor();
            Colors.MenuHeaderFont = ColorSchemes[ColorSchemeID]["MenuHeaderFont"].ToColor();
            Colors.MenuItemFont = ColorSchemes[ColorSchemeID]["MenuItemFont"].ToColor();
            Colors.TextBlockFont = ColorSchemes[ColorSchemeID]["TextBlockFont"].ToColor();
            Colors.ButtonFont = ColorSchemes[ColorSchemeID]["ButtonFont"].ToColor();
            Colors.ToolTipFont = ColorSchemes[ColorSchemeID]["ToolTipFont"].ToColor();
            Colors.ListBoxItemInactiveFont = ColorSchemes[ColorSchemeID]["ListBoxItemInactiveFont"].ToColor();
            Colors.ListBoxItemActiveFont = ColorSchemes[ColorSchemeID]["ListBoxItemActiveFont"].ToColor();
        }

        private void ApplyButtonStyle()
        {
            Colors.ButtonIdleBackground = ColorSchemes[ColorSchemeID]["ButtonIdleBackground"].ToColor();
            Colors.ButtonIdleBorder = ColorSchemes[ColorSchemeID]["ButtonIdleBorder"].ToColor();
            Colors.ButtonHoverBackground = ColorSchemes[ColorSchemeID]["ButtonHoverBackground"].ToColor();
            Colors.ButtonHoverBorder = ColorSchemes[ColorSchemeID]["ButtonHoverBorder"].ToColor();
            Colors.ButtonClickBackground = ColorSchemes[ColorSchemeID]["ButtonClickBackground"].ToColor();
            Colors.ButtonClickBorder = ColorSchemes[ColorSchemeID]["ButtonClickBorder"].ToColor();

            Colors.ToggleButtonHover = ColorSchemes[ColorSchemeID]["ToggleButtonHover"].ToColor();
            Colors.ToggleButtonIdle = ColorSchemes[ColorSchemeID]["ToggleButtonIdle"].ToColor();
            Colors.ToggleButtonChecked = ColorSchemes[ColorSchemeID]["ToggleButtonChecked"].ToColor();
        }

        private void ApplyCheckBoxStyle()
        {
            Colors.CheckBoxBorderIdle = ColorSchemes[ColorSchemeID]["CheckBoxBorderIdle"].ToColor();
            Colors.CheckBoxBorderHover = ColorSchemes[ColorSchemeID]["CheckBoxBorderHover"].ToColor();
            Colors.CheckBoxFill = ColorSchemes[ColorSchemeID]["CheckBoxFill"].ToColor();
        }

        private void ApplyControlStyle()
        {
            Colors.ControlIdleBorder = ColorSchemes[ColorSchemeID]["ControlIdleBorder"].ToColor();
            Colors.ControlHoverBorder = ColorSchemes[ColorSchemeID]["ControlHoverBorder"].ToColor();
            Colors.ControlDisabled = ColorSchemes[ColorSchemeID]["ControlDisabled"].ToColor();

            Colors.ToolTipBackground = ColorSchemes[ColorSchemeID]["ToolTipBackground"].ToColor();
            Colors.ToolTipBorder = ColorSchemes[ColorSchemeID]["ToolTipBorder"].ToColor();

            Colors.ListBoxBorder = ColorSchemes[ColorSchemeID]["ListBoxBorder"].ToColor();
            Colors.ListBoxBackground = ColorSchemes[ColorSchemeID]["ListBoxBackground"].ToColor();
            Colors.ListBoxItemHoverBackground = ColorSchemes[ColorSchemeID]["ListBoxItemHoverBackground"].ToColor();
            Colors.ListBoxItemActiveSelectionBackground = ColorSchemes[ColorSchemeID]["ListBoxItemActiveSelectionBackground"].ToColor();
            Colors.ListBoxItemInactiveSelectionBackground = ColorSchemes[ColorSchemeID]["ListBoxItemInactiveSelectionBackground"].ToColor();
        }

        private void ApplyWindowStyle()
        {
            Colors.WindowBackground = ColorSchemes[ColorSchemeID]["WindowBackground"].ToColor();
            Colors.WindowActiveBorder = ColorSchemes[ColorSchemeID]["WindowActiveBorder"].ToColor();
            Colors.WindowActiveGlow = ColorSchemes[ColorSchemeID]["WindowActiveGlow"].ToColor();
            Colors.WindowInactiveBorder = ColorSchemes[ColorSchemeID]["WindowInactiveBorder"].ToColor();
            Colors.WindowInactiveGlow = ColorSchemes[ColorSchemeID]["WindowInactiveGlow"].ToColor();
            Colors.WindowSeparator = ColorSchemes[ColorSchemeID]["WindowSeparator"].ToColor();


            Colors.CaptionButtonIdle = ColorSchemes[ColorSchemeID]["CaptionButtonIdle"].ToColor();
            Colors.CaptionButtonHover = ColorSchemes[ColorSchemeID]["CaptionButtonHover"].ToColor();
            Colors.PanelHideButtonIdle = ColorSchemes[ColorSchemeID]["PanelHideButtonIdle"].ToColor();
            Colors.PanelHideButtonHover = ColorSchemes[ColorSchemeID]["PanelHideButtonHover"].ToColor();

            Colors.GroupBoxBorder = ColorSchemes[ColorSchemeID]["GroupBoxBorder"].ToColor();
        }

        private void ApplyMessageControlStyle()
        {
            Colors.IncomingMessageBackground = ColorSchemes[ColorSchemeID]["IncomingMessageBackground"].ToColor();
            Colors.IncomingMessageBorder = ColorSchemes[ColorSchemeID]["IncomingMessageBorder"].ToColor();
            Colors.OutgoingMessageBackground = ColorSchemes[ColorSchemeID]["OutgoingMessageBackground"].ToColor();
            Colors.OutgoingMessageBorder = ColorSchemes[ColorSchemeID]["OutgoingMessageBorder"].ToColor();
            Colors.TopicMessageBackground = ColorSchemes[ColorSchemeID]["TopicMessageBackground"].ToColor();
            Colors.TopicMessageBorder = ColorSchemes[ColorSchemeID]["TopicMessageBorder"].ToColor();
        }

        private void ApplyScrollbarStyle()
        {
            Colors.ScrollbarIdleForeground = ColorSchemes[ColorSchemeID]["ScrollBarIdleForeground"].ToColor();
            Colors.ScrollbarHoverForeground = ColorSchemes[ColorSchemeID]["ScrollBarHoverForeground"].ToColor();
            Colors.ScrollbarClickForeground = ColorSchemes[ColorSchemeID]["ScrollBarClickForeground"].ToColor();
        }

        private void ApplyMenuStyle()
        {
            Colors.MenuHighlight = ColorSchemes[ColorSchemeID]["MenuHighlight"].ToColor();
            Colors.MenuBackground = ColorSchemes[ColorSchemeID]["MenuBackground"].ToColor();
            Colors.MenuBorder = ColorSchemes[ColorSchemeID]["MenuBorder"].ToColor();

            Colors.MainMenuButtonIdle = ColorSchemes[ColorSchemeID]["MainMenuButtonIdle"].ToColor();
            Colors.MainMenuButtonHover = ColorSchemes[ColorSchemeID]["MainMenuButtonHover"].ToColor();
        }
    }
}
