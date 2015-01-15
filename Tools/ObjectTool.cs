﻿namespace SadConsoleEditor.Tools
{
    using Microsoft.Xna.Framework;
    using SadConsole;
    using SadConsole.Input;
    using Panels;
    using System.Collections.Generic;
    using System.Linq;
    using SadConsole.GameHelpers;

    class ObjectTool : ITool
    {
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

        private EntityBrush _brush;
        private Panels.ObjectToolPanel _panel;

        private GameObject _currentGameObject;

        public ObjectTool()
        {
            _panel = new Panels.ObjectToolPanel();
            ControlPanels = new CustomPanel[] { _panel };

            _brush = new EntityBrush();
        }

        public override string ToString()
        {
            return Title;
        }

        public void OnSelected()
        {
            EditorConsoleManager.Instance.UpdateBrush(_brush);
            if (_panel.SelectedObject != null)
            {
                _currentGameObject = _panel.SelectedObject;

                _brush.CurrentAnimation.Frames[0].Fill(_currentGameObject.Character.Foreground,
                                   _currentGameObject.Character.Background,
                                   _currentGameObject.Character.CharacterIndex, null,
                                   _currentGameObject.Character.SpriteEffect);
            }
            else
            {
                _currentGameObject = null;
                _brush.CurrentAnimation.Frames[0].Fill(Color.White,
                                   Color.Transparent,
                                   0, null);
            }

            _brush.IsVisible = false;
            if (EditorConsoleManager.Instance.SelectedEditor is Editors.GameScreenEditor)
            {
                var editor = (Editors.GameScreenEditor)EditorConsoleManager.Instance.SelectedEditor;
                editor.DisplayObjectLayer = true;
            }
        }


        public void OnDeselected()
        {
            if (EditorConsoleManager.Instance.SelectedEditor is Editors.GameScreenEditor)
            {
                var editor = (Editors.GameScreenEditor)EditorConsoleManager.Instance.SelectedEditor;
                editor.DisplayObjectLayer = false;
            }
        }

        public void RefreshTool()
        {
            if (_panel.SelectedObject != null)
            {
                _currentGameObject = _panel.SelectedObject;

                _brush.CurrentAnimation.Frames[0].Fill(_currentGameObject.Character.Foreground,
                                   _currentGameObject.Character.Background,
                                   _currentGameObject.Character.CharacterIndex, null,
                                   _currentGameObject.Character.SpriteEffect);
            }
            else
            {
                _currentGameObject = null;
                _brush.CurrentAnimation.Frames[0].Fill(Color.White,
                                   Color.Transparent,
                                   0, null);
            }
        }

        public void ProcessKeyboard(KeyboardInfo info, CellSurface surface)
        {

        }

        public void ProcessMouse(MouseInfo info, CellSurface surface)
        {

        }

        public void MouseEnterSurface(MouseInfo info, CellSurface surface)
        {
            _brush.IsVisible = true;
        }

        public void MouseExitSurface(MouseInfo info, CellSurface surface)
        {
            _brush.IsVisible = false;
        }

        public void MouseMoveSurface(MouseInfo info, CellSurface surface)
        {
            _brush.IsVisible = true;
            _brush.Position = info.ConsoleLocation;

            if (EditorConsoleManager.Instance.SelectedEditor is Editors.GameScreenEditor)
            {
                var editor = (Editors.GameScreenEditor)EditorConsoleManager.Instance.SelectedEditor;
                var point = new Point(info.ConsoleLocation.X, info.ConsoleLocation.Y);

                if (info.LeftClicked)
                {
                    // Suck up the object
                    if (Engine.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                    {
                        if (editor.GameObjects.ContainsKey(point))
                        {
                            _currentGameObject = editor.GameObjects[point].Clone();

                            _brush.CurrentAnimation.Frames[0].Fill(_currentGameObject.Character.Foreground,
                                   _currentGameObject.Character.Background,
                                   _currentGameObject.Character.CharacterIndex, null,
                                   _currentGameObject.Character.SpriteEffect);
                        }
                    }

                    if (Engine.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt))
                    {
                        if (editor.GameObjects.ContainsKey(point))
                        {
                            _currentGameObject = editor.GameObjects[point].Clone();
                            _panel.AddNewGameObject(_currentGameObject);
                        }
                    }

                    // Delete the object
                    else if (Engine.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                    {
                        if (editor.GameObjects.ContainsKey(point))
                            editor.GameObjects.Remove(point);

                        editor.SyncObjectsToLayer();
                    }
                }

                // Stamp object
                else if (info.LeftButtonDown)
                {
                    if (_currentGameObject != null && 
                        !Engine.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift) && 
                        !Engine.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt) &&
                        !Engine.Keyboard.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                    {
                        var cell = surface[info.ConsoleLocation.X, info.ConsoleLocation.Y];

                        if (editor.GameObjects.ContainsKey(point))
                            editor.GameObjects.Remove(point);

                        var gameObj = _currentGameObject.Clone();
                        gameObj.Position = point;

                        editor.GameObjects.Add(point, gameObj);
                        editor.SyncObjectsToLayer();
                    }
                }

                // Edit object
                else if (info.RightClicked)
                {
                    if (editor.GameObjects.ContainsKey(point))
                    {
                        Windows.EditObjectPopup popup = new Windows.EditObjectPopup(editor.GameObjects[point]);
                        popup.Closed += (o, e) => { if (popup.DialogResult) editor.SyncObjectsToLayer(); };
                        popup.Show(true);
                    }
                }
            }
        }
    }
}
