﻿using System;
using SadConsole.Surfaces;
using System.Linq;

namespace SadConsoleEditor.FileLoaders
{
    class GameObject : IFileLoader
    {
        public bool SupportsLoad { get { return true; } }

        public bool SupportsSave { get { return true; } }

        public string[] Extensions
        {
            get
            {
                return new string[] { "entity", "object" };
            }
        }

        public string FileTypeName
        {
            get
            {
                return "Game Object";
            }
        }

        public object Load(string file)
        {
            return SadConsole.GameHelpers.GameObject.Load(file);
        }

        public void Save(object surface, string file)
        {
            ((SadConsole.GameHelpers.GameObject)surface).Save(file);
        }
    }
}
