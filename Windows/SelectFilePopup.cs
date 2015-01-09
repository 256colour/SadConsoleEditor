﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SadConsole;
using Microsoft.Xna.Framework;
using SadConsole.Controls;
using SadConsole.Consoles;
namespace SadConsoleEditor.Windows
{
    class SelectFilePopup : Window
    {
        #region Fields
        private string _currentFolder;
        private SadEditor.Controls.FileDirectoryListbox _directoryListBox;
        private InputBox _fileName;
        private Button _selectButton;
        private Button _cancelButton;
        #endregion

        #region Properties
        public string CurrentFolder
        {
            get { return _directoryListBox.CurrentFolder; }
            set { _directoryListBox.CurrentFolder = value; }
        }

        public string FileFilter
        {
            get { return _directoryListBox.FileFilter; }
            set { _directoryListBox.FileFilter = value; }
        }

        public string PreferredExtensions
        {
            get { return _directoryListBox.HighlightedExtentions; }
            set { _directoryListBox.HighlightedExtentions = value; }
        }

        public string SelectedFile { get; private set; }

        public bool SkipFileExistCheck { get; set; }

        public string SelectButtonText { get { return _selectButton.Text; } set { _selectButton.Text = value; } }
        #endregion

        #region Constructors

        public SelectFilePopup()
            : base(50, 30)
        {
            Title = "Select File";
            _directoryListBox = new SadEditor.Controls.FileDirectoryListbox()
            {
                Position = new Point(1, 1),
                HideBorder = true
            };
            _directoryListBox.HighlightedExtentions = ".con;.console;.brush";
            _directoryListBox.Resize(this.CellData.Width - 2, this.CellData.Height - 5);
            _directoryListBox.SelectedItemChanged += _directoryListBox_SelectedItemChanged;
            _directoryListBox.SelectedItemExecuted += _directoryListBox_SelectedItemExecuted;

            _fileName = new InputBox(this.CellData.Width - 11)
            {
                Position = new Point(2, this.CellData.Height - 3),
            };
            _fileName.TextChanged += _fileName_TextChanged;

            _selectButton = new Button(6, 1)
            {
                Text = "Open",
                Position = new Point(this.CellData.Width - 8, this.CellData.Height - 3),
                IsEnabled = false
            };
            _selectButton.ButtonClicked += new EventHandler(_selectButton_Action);

            _cancelButton = new Button(6, 1)
            {
                Text = "Cancel",
                Position = new Point(this.CellData.Width - 8, this.CellData.Height - 2)
            };
            _cancelButton.ButtonClicked += new EventHandler(_cancelButton_Action);

            Add(_directoryListBox);
            Add(_fileName);
            Add(_selectButton);
            Add(_cancelButton);
        }
        #endregion

        public override void Show(bool modal)
        {
            SelectedFile = "";
            _cellData.Print(2, _cellData.Height - 2, FileFilter.Replace(';', ' ').Replace("*", ""));

            base.Show(modal);
        }

        void _cancelButton_Action(object sender, EventArgs e)
        {
            DialogResult = false;
            Hide();
        }

        void _selectButton_Action(object sender, EventArgs e)
        {
            if (_fileName.Text != string.Empty)
            {
                SelectedFile = System.IO.Path.Combine(_directoryListBox.CurrentFolder, _fileName.Text);

                var extensions = FileFilter.Replace("*", "").Split(';');
                bool foundExtension = false;
                foreach (var item in extensions)
                {
                    if (SelectedFile.ToLower().EndsWith(item))
                    {
                        foundExtension = true;
                        break;
                    }
                }

                if (!foundExtension)
                    SelectedFile += extensions[0];

                DialogResult = true;
                Hide();
            }
        }

        void _directoryListBox_SelectedItemExecuted(object sender, SadEditor.Controls.FileDirectoryListbox.SelectedItemEventArgs e)
        {

        }

        void _directoryListBox_SelectedItemChanged(object sender, SadEditor.Controls.FileDirectoryListbox.SelectedItemEventArgs e)
        {
            if (e.Item is System.IO.FileInfo)
                _fileName.Text = ((System.IO.FileInfo)e.Item).Name;
            else if (e.Item is SadEditor.Controls.HighlightedExtFile)
                _fileName.Text = ((SadEditor.Controls.HighlightedExtFile)e.Item).Name;
            else
                _fileName.Text = "";
        }

        void _fileName_TextChanged(object sender, EventArgs e)
        {
            _selectButton.IsEnabled = _fileName.Text != "" && (SkipFileExistCheck || System.IO.File.Exists(System.IO.Path.Combine(_directoryListBox.CurrentFolder, _fileName.Text)));
        }

        public override void Redraw()
        {
            base.Redraw();

            if (_directoryListBox != null)
                _cellData.Print(2, _cellData.Height - 2, FileFilter.Replace(';', ' ').Replace("*", ""));
        }
    }
}
