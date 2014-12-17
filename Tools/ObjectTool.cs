﻿namespace SadConsoleEditor.Tools
{
    using Microsoft.Xna.Framework;
    using SadConsole;
    using SadConsole.Consoles;
    using SadConsole.Input;

    class ObjectTool : ITool
    {
        private bool writing;
        private Console tempConsole;
        private int _cursorCharacter = 95;

        private EntityBrush _brush;

        public const string ID = "OBJECT";
        public string Id
        {
            get { return ID; }
        }

        public string Title
        {
            get { return "Object"; }
        }

        public CustomPanel[] ControlPanels { get; private set; }

        private ObjectToolPanel _objectSettingsPanel;

        public override string ToString()
        {
            return Title;
        }

        public ObjectTool()
        {

            _objectSettingsPanel = new ObjectToolPanel();
            this.ControlPanels = new CustomPanel[] { _objectSettingsPanel };
        }

        public void OnSelected()
        {
            SadConsole.Effects.Blink blinkEffect = new SadConsole.Effects.Blink();
            blinkEffect.BlinkSpeed = 0.35f;
            _brush = new EntityBrush();

            EditorConsoleManager.Instance.UpdateBrush(_brush);
            _brush.CurrentAnimation.Frames[0].Fill(Color.White, Color.Black, _cursorCharacter, blinkEffect);
            _brush.IsVisible = false;
        }

        public void OnDeselected()
        {
            writing = false;
        }

        public void RefreshTool()
        {
            EditorConsoleManager.Instance.ToolPane.KeyboardHandler = null;
        }

        public void ProcessKeyboard(KeyboardInfo info, CellSurface surface)
        {
            
        }

        public void ProcessMouse(MouseInfo info, CellSurface surface)
        {
            if (info.LeftClicked)
            {
                writing = true;

                tempConsole.CellData = surface;
                tempConsole.VirtualCursor.Position = _brush.Position = info.ConsoleLocation;

                _brush.IsVisible = true;
            }
        }

        public void MouseEnterSurface(MouseInfo info, CellSurface surface)
        {

        }

        public void MouseExitSurface(MouseInfo info, CellSurface surface)
        {
        }

        public void MouseMoveSurface(MouseInfo info, CellSurface surface)
        {
        }
    }
}
