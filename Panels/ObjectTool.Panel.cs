﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole.Consoles;
using SadConsole.Controls;
using SadConsole.Input;
using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.GameHelpers;

namespace SadConsoleEditor.Panels
{
    class ObjectToolPanel : CustomPanel
    {
        ListBox<GameObjectListBoxItem> _objectTypesListbox;
        Button _createNewObjectButton;
        Button _editObjectButton;
        Button _deleteObjectButton;
        Button _exportListButton;

        Button _deleteAllFromMap;
        CheckBox _showOnlySelected;
        CheckBox _showUnknownObjects;

        public GameObject SelectedObject;

        public ObjectToolPanel()
        {
            Title = "Object Types";

            _objectTypesListbox = new ListBox<GameObjectListBoxItem>(Consoles.ToolPane.PanelWidth, 10);
            _createNewObjectButton = new Button(Consoles.ToolPane.PanelWidth, 1);
            _editObjectButton = new Button(Consoles.ToolPane.PanelWidth, 1);
            _deleteObjectButton = new Button(Consoles.ToolPane.PanelWidth, 1);
            _exportListButton = new Button(Consoles.ToolPane.PanelWidth, 1);

            _objectTypesListbox.SelectedItemChanged += _objectTypesListbox_SelectedItemChanged;
            _createNewObjectButton.ButtonClicked += _createNewObjectButton_ButtonClicked;
            _editObjectButton.ButtonClicked += _editObjectButton_ButtonClicked;
            _deleteObjectButton.ButtonClicked += _deleteObjectButton_ButtonClicked;
            _exportListButton.ButtonClicked += _exportListButton_ButtonClicked;

            _editObjectButton.IsEnabled = false;
            _deleteObjectButton.IsEnabled = false;

            _objectTypesListbox.HideBorder = true;

            _createNewObjectButton.Text = "Create New Type";
            _editObjectButton.Text = "Edit Type";
            _deleteObjectButton.Text = "Delete Type";
            _exportListButton.Text = "Save System List";

            Controls = new ControlBase[] { _objectTypesListbox, _createNewObjectButton, _editObjectButton, _deleteObjectButton, _exportListButton };

            // Load the known object types.
            if (System.IO.File.Exists(Settings.FileObjectTypes))
            {
                using (var fileObject = System.IO.File.OpenRead(Settings.FileObjectTypes))
                {
                    var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(GameObject[]));

                    var gameObjects = serializer.ReadObject(fileObject) as GameObject[];

                    foreach (var item in gameObjects)
                    {
                        var newItem = new GameObjectMeta(item, true);
                        _objectTypesListbox.Items.Add(newItem);
                    }
                }
            }

            _exportListButton.IsEnabled = _objectTypesListbox.Items.Count != 0;
        }

        void _exportListButton_ButtonClicked(object sender, EventArgs e)
        {
            // Save known object types
            if (System.IO.File.Exists(Settings.FileObjectTypes))
                System.IO.File.Delete(Settings.FileObjectTypes);

            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(GameObject[]), new System.Type[] { typeof(GameObject) });

            using (var stream = System.IO.File.OpenWrite(Settings.FileObjectTypes))
                serializer.WriteObject(stream, _objectTypesListbox.Items.Cast<GameObjectMeta>().Select<GameObjectMeta, GameObject>((o) => o.BackingObject).ToArray());
        }

        void _deleteObjectButton_ButtonClicked(object sender, EventArgs e)
        {
            Window.Prompt(new ColoredString("Are you sure? You will need to manually clear the objects on your map."), "Yes", "No", (r) =>
            {
                if (r) _objectTypesListbox.Items.Remove(_objectTypesListbox.SelectedItem);
                _exportListButton.IsEnabled = _objectTypesListbox.Items.Count != 0;
            });
        }

        void _editObjectButton_ButtonClicked(object sender, EventArgs e)
        {
            Windows.EditObjectPopup popup = new Windows.EditObjectPopup(((GameObjectMeta)_objectTypesListbox.SelectedItem).BackingObject);
            popup.Closed += (o, e2) =>
                {
                    if (popup.DialogResult)
                    {
                        _objectTypesListbox.GetContainer(_objectTypesListbox.SelectedItem).IsDirty = true;
                        EditorConsoleManager.Instance.ToolPane.SelectedTool.RefreshTool();
                    }
                };
            popup.Show(true);
        }

        void _createNewObjectButton_ButtonClicked(object sender, EventArgs e)
        {
            GameObject newGameObject = new GameObject();
            Windows.EditObjectPopup popup = new Windows.EditObjectPopup(newGameObject);
            popup.Closed += (o, e2) =>
                {
                    if (popup.DialogResult)
                    {
                        GameObjectMeta meta = new GameObjectMeta(newGameObject, false);
                        _objectTypesListbox.Items.Add(meta);
                        _objectTypesListbox.SelectedItem = meta;
                        _exportListButton.IsEnabled = _objectTypesListbox.Items.Count != 0;
                        EditorConsoleManager.Instance.ToolPane.SelectedTool.RefreshTool();
                    }
                };

            popup.Show(true);
        }

        void _objectTypesListbox_SelectedItemChanged(object sender, ListBox<GameObjectListBoxItem>.SelectedItemEventArgs e)
        {
            if (_objectTypesListbox.SelectedItem == null)
                SelectedObject = null;
            else
                SelectedObject = ((GameObjectMeta)_objectTypesListbox.SelectedItem).BackingObject;
            
            _editObjectButton.IsEnabled = SelectedObject != null;
            _deleteObjectButton.IsEnabled = SelectedObject != null;
            EditorConsoleManager.Instance.ToolPane.SelectedTool.RefreshTool();
        }

        public override void ProcessMouse(MouseInfo info)
        {
            
        }

        public override int Redraw(ControlBase control)
        {
            if (control == _objectTypesListbox || control == _deleteObjectButton)
                return 1;

            return 0;
        }
        public override void Loaded()
        {
        }

        public void AddNewGameObject(GameObject gameObject)
        {
            var newItem = new GameObjectMeta(gameObject.Clone(), false);
            _objectTypesListbox.Items.Add(newItem);
            _objectTypesListbox.SelectedItem = newItem;
        }
    }

	public class DisplayObjectToolPanel : CustomPanel
	{
		private Controls.ColorPresenter _foregroundPresenter;
		private Controls.ColorPresenter _backgroundPresenter;
		private Controls.ColorPresenter _characterPresenter;
		private DrawingSurface _propertySurface;

		GameObject _displayedObject;

        public GameObject DisplayedObject
		{
			get { return _displayedObject; }
			set
			{
				_displayedObject = value;

				if (value != null)
				{
					_foregroundPresenter.SelectedColor = value.Character.Foreground;
					_backgroundPresenter.SelectedColor = value.Character.Background;
					_characterPresenter.SelectedColor = value.Character.Background;
					_characterPresenter.Character = value.Character.CharacterIndex;
					_characterPresenter.CharacterColor = value.Character.Foreground;

					
                    _propertySurface.Clear();
					if (value.Settings.Count == 0)
						_propertySurface.Resize(18, 1);
					else
						_propertySurface.Resize(18, value.Settings.Count * 2);

					int y = 0;
					foreach (var setting in value.Settings)
					{
						_propertySurface.Print(0, y, setting.Name.Length > 18 ? setting.Name.Substring(0, 18) : setting.Name, Settings.Yellow);
						_propertySurface.Print(1, y + 1, setting.Value.Length > 17 ? setting.Value.Substring(0, 17) : setting.Value, Settings.Grey);


						y += 2;
					}

					IsVisible = true;
				}
				else
					IsVisible = false;
			}
		}

		public DisplayObjectToolPanel(string title)
		{
			Title = title;

			_foregroundPresenter = new SadConsoleEditor.Controls.ColorPresenter("Foreground", Settings.Green, 18);
			_backgroundPresenter = new SadConsoleEditor.Controls.ColorPresenter("Background", Settings.Green, 18);
			_characterPresenter = new SadConsoleEditor.Controls.ColorPresenter("Preview", Settings.Green, 18);

			_propertySurface = new DrawingSurface(18, 1);

			Controls = new ControlBase[] { _foregroundPresenter, _backgroundPresenter, _characterPresenter, _propertySurface };
		}

		
		public override void ProcessMouse(MouseInfo info)
		{

		}

		public override int Redraw(ControlBase control)
		{

			if (control == _characterPresenter)
				return 1;

			return 0;
		}
		public override void Loaded()
		{
		}
		
	}

}
