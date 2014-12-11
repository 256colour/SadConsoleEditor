﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SadConsole;

namespace SadConsoleEditor
{
    public static class Settings
    {
        #region Config
        public static int BoundsWidth = 80;
        public static int BoundsHeight = 25;
        public static Font ScreenFont;
        #endregion

        #region Colors
        public static Color Green = new Color(165, 224, 45);
        public static Color Red = new Color(246, 38, 108);
        public static Color Blue = new Color(100, 217, 234);
        public static Color Grey = new Color(117, 111, 81);
        public static Color Yellow = new Color(226, 218, 110);
        public static Color Orange = new Color(251, 149, 31);

        public static Color Color_MenuBack = new Color(39, 40, 34);
        public static Color Color_MenuBackDark = new Color(Color.DarkBlue.R - 40, Color.DarkBlue.G - 40, Color.DarkBlue.B - 40, Color.DarkBlue.A);
        public static Color Color_MenuShade = Color.DarkSlateGray; //new Color(Color.DarkBlue.R - 40, Color.DarkBlue.G - 40, Color.DarkBlue.B - 40, Color.DarkBlue.A);
        public static Color Color_MenuLines = Color.Gray;
        public static Color Color_MenuLinesBright = Color.White;
        public static Color Color_TitleText = Orange;
        public static Color Color_WorkAreaBack = new Color(20, 20, 20);
            
        public static Color Color_TextBright = Color.White;
        public static Color Color_Text = Grey;
        public static Color Color_TextSelected = Color.Yellow;
        public static Color Color_TextSelectedDark  = ClearAlpha(Color.Yellow * 0.9f);
        public static Color Color_TextDim = Color.Gray;
        public static Color Color_TextDark = ClearAlpha(Color.Gray * 0.7f);
        public static Color Color_ControlBack = ClearAlpha(Color_MenuBack * 0.8f);
        public static Color Color_ControlBackDim = ClearAlpha(Color_MenuShade * 0.3f);
        public static Color Color_ControlBackSelected = Color_MenuShade;

        public static SadConsole.CellAppearance Appearance_ListBoxItem_Normal = new SadConsole.CellAppearance(Color_Text, Color_ControlBack);
        public static SadConsole.CellAppearance Appearance_ListBoxItem_SelectedItem = new SadConsole.CellAppearance(Yellow, Color_ControlBack);


        public static SadConsole.CellAppearance Appearance_ControlNormal = new SadConsole.CellAppearance(Color_Text, Color_ControlBack);
        public static SadConsole.CellAppearance Appearance_ControlDisabled = new SadConsole.CellAppearance(Color_TextDim, Color_ControlBackDim);
        public static SadConsole.CellAppearance Appearance_ControlOver = new SadConsole.CellAppearance(Color_TextSelectedDark, Color_ControlBackSelected);
        public static SadConsole.CellAppearance Appearance_ControlSelected = new SadConsole.CellAppearance(Color_TextSelected, Color_ControlBackSelected);
        public static SadConsole.CellAppearance Appearance_ControlMouseDown = new SadConsole.CellAppearance(Color_ControlBackSelected, Color_TextSelectedDark);
        public static SadConsole.CellAppearance Appearance_ControlFocused = new SadConsole.CellAppearance(Color_ControlBack, Color_TextDark);

        public static SadConsole.CellAppearance Appearance_ControlTextBoxNormal = new SadConsole.CellAppearance(Color_Text, Color_ControlBack);

        public static SadConsole.Themes.RadioButtonTheme NoCheckRadioButtonTheme;
        #endregion

        #region Alternate Themes
        #endregion

        private static Color ClearAlpha(Color color)
        {
            color.A = 255;
            return color;
        }

        public static void SetupThemes()
        {
            SadConsole.Themes.Library.Default.RadioButtonTheme.Button.Normal = Appearance_ControlNormal;
            SadConsole.Themes.Library.Default.RadioButtonTheme.Button.MouseOver = Appearance_ControlOver;
            SadConsole.Themes.Library.Default.RadioButtonTheme.Button.Selected = Appearance_ControlNormal;
            SadConsole.Themes.Library.Default.RadioButtonTheme.Button.Focused = Appearance_ControlNormal;

            SadConsole.Themes.Library.Default.RadioButtonTheme.Normal = Appearance_ControlNormal;
            SadConsole.Themes.Library.Default.RadioButtonTheme.MouseOver = Appearance_ControlOver;
            SadConsole.Themes.Library.Default.RadioButtonTheme.Selected = Appearance_ListBoxItem_SelectedItem;
            SadConsole.Themes.Library.Default.RadioButtonTheme.Focused = Appearance_ControlSelected;

            SadConsole.Themes.Library.Default.CheckBoxTheme.Button.Normal = Appearance_ControlNormal;
            SadConsole.Themes.Library.Default.CheckBoxTheme.Button.MouseOver = Appearance_ControlOver;
            SadConsole.Themes.Library.Default.CheckBoxTheme.Button.Selected = Appearance_ControlNormal;
            SadConsole.Themes.Library.Default.CheckBoxTheme.Button.Focused = Appearance_ControlNormal;

            SadConsole.Themes.Library.Default.CheckBoxTheme.Normal = Appearance_ControlNormal;
            SadConsole.Themes.Library.Default.CheckBoxTheme.MouseOver = Appearance_ControlOver;
            SadConsole.Themes.Library.Default.CheckBoxTheme.Selected = Appearance_ListBoxItem_SelectedItem;
            SadConsole.Themes.Library.Default.CheckBoxTheme.Focused = Appearance_ControlSelected;

            NoCheckRadioButtonTheme = (SadConsole.Themes.RadioButtonTheme)SadConsole.Themes.Library.Default.RadioButtonTheme.Clone();
            NoCheckRadioButtonTheme.Button.Selected = Appearance_ControlSelected;
            NoCheckRadioButtonTheme.Selected = Appearance_ControlSelected;

            SadConsole.Themes.Library.Default.ButtonTheme.Normal = Appearance_ControlNormal;
            SadConsole.Themes.Library.Default.ButtonTheme.Disabled = Appearance_ControlDisabled;
            SadConsole.Themes.Library.Default.ButtonTheme.MouseOver = Appearance_ControlOver;
            SadConsole.Themes.Library.Default.ButtonTheme.MouseClicking = Appearance_ControlMouseDown;
            SadConsole.Themes.Library.Default.ButtonTheme.Focused = Appearance_ControlSelected;

            SadConsole.Themes.Library.Default.InputBoxTheme.Normal = Appearance_ControlTextBoxNormal;
            SadConsole.Themes.Library.Default.InputBoxTheme.Focused = Appearance_ListBoxItem_SelectedItem;
            SadConsole.Themes.Library.Default.InputBoxTheme.MouseOver = Appearance_ControlOver;

            SadConsole.Themes.Library.Default.ListBoxTheme.Border.Background = Color_ControlBack;
            SadConsole.Themes.Library.Default.ListBoxTheme.Border.Foreground = Color_Text;
            SadConsole.Themes.Library.Default.ListBoxTheme.Normal = Appearance_ControlNormal;
            SadConsole.Themes.Library.Default.ListBoxTheme.MouseOver = Appearance_ControlOver;
            SadConsole.Themes.Library.Default.ListBoxTheme.Item.Selected = Appearance_ListBoxItem_SelectedItem;
            SadConsole.Themes.Library.Default.ListBoxTheme.Item.Normal = Appearance_ListBoxItem_Normal;
            SadConsole.Themes.Library.Default.ListBoxTheme.Item.MouseOver = Appearance_ControlOver;
            SadConsole.Themes.Library.Default.ListBoxTheme.ScrollBarTheme.Bar.Normal = Appearance_ControlNormal;
            SadConsole.Themes.Library.Default.ListBoxTheme.ScrollBarTheme.Ends.Normal = Appearance_ControlNormal;
            SadConsole.Themes.Library.Default.ListBoxTheme.ScrollBarTheme.Slider.Normal = Appearance_ControlNormal;

            SadConsole.Themes.Library.Default.WindowTheme.BorderStyle.Background = Settings.Color_MenuBack;
            SadConsole.Themes.Library.Default.WindowTheme.BorderStyle.Foreground = Color_Text;
            SadConsole.Themes.Library.Default.WindowTheme.TitleStyle.Background = Settings.Color_MenuBack;
            SadConsole.Themes.Library.Default.WindowTheme.TitleStyle.Foreground = Color_TitleText;
            SadConsole.Themes.Library.Default.WindowTheme.FillStyle.Background = Settings.Color_MenuBack;
            SadConsole.Themes.Library.Default.WindowTheme.FillStyle.Foreground = Color_Text;

            SadConsole.Themes.Library.Default.ScrollBarTheme.Bar.Normal = Appearance_ControlNormal;
            SadConsole.Themes.Library.Default.ScrollBarTheme.Ends.Normal = Appearance_ControlNormal;
            SadConsole.Themes.Library.Default.ScrollBarTheme.Slider.Normal = Appearance_ControlNormal;
            
        }
    }


    public enum ToolTypes
    {
        Animation,
        Console,
        Objects,
    }

    public enum ConsoleTools
    {
        Paint,
        Dropper,
        Gradient,
        ConsolePlacer,
        SelectArea,
        Text,
        Colorizer,
        Line
    }
}
