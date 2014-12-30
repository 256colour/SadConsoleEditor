﻿using System;
using Console = SadConsole.Consoles.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SadConsole.Consoles;
using SadConsole;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using SadConsole.Input;

namespace SadConsoleEditor.Consoles
{
    [DataContract]
    public class LayeredConsole: Console
    {
        [DataContract]
        public class Metadata
        {
            [DataMember]
            public string Name;
            [DataMember]
            public bool IsVisible = true;
            [DataMember]
            public bool IsRemoveable = true;
            [DataMember]
            public bool IsMoveable = true;
            [DataMember]
            public bool IsRenamable = true;

            public int Index;

            public override string ToString()
            {
                return Name;
            }
        }

        [DataMember]
        public int Width { get; protected set; }
        [DataMember]
        public int Height { get; protected set; }

        public int Layers { get { return _layers.Count; } }

        [IgnoreDataMember]
        public CellSurface ActiveLayer { get; protected set; }

        [DataMember(Name = "LayerMetadata")]
        private List<Metadata> _layerMetadata;

        [DataMember(Name = "Layers")]
        protected List<CellsRenderer> _layers;

        public CellsRenderer this[int index]
        {
            get { return _layers[index]; }
        }

        public LayeredConsole(int layers, int width, int height): base(width, height)
        {
            Width = width;
            Height = height;

            _layers = new List<CellsRenderer>();
            _layerMetadata = new List<Metadata>();
            
            for (int i = 0; i < layers; i++)
                AddLayer(i.ToString());


            SetActiveLayer(0);
        }

        public virtual void SyncLayers()
        {
            if (_layers != null)
                foreach (var item in _layers)
                {
                    item.Position = this.Position;
                    item.Font = this.Font;
                }
        }

        protected override void OnFontChanged()
        {
            SyncLayers();
        }

        public void Clear(Color foreground, Color background)
        {
            // Create all layers
            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].CellData.DefaultBackground = background;
                _layers[i].CellData.DefaultForeground = foreground;
                _layers[i].CellData.Clear();
            }
        }

        public void SetActiveLayer(int index)
        {
            if (index < 0 || index > _layers.Count - 1)
                throw new ArgumentOutOfRangeException("index");

            _cellData = _layers[index].CellData;
            ActiveLayer = _cellData;
            ResetViewArea();
        }

        public virtual void Resize(int width, int height)
        {
            Width = width;
            Height = height;

            for (int i = 0; i < _layers.Count; i++)
                _layers[i].CellData.Resize(width, height);

            ResetViewArea();
        }

        public void Move(Point position)
        {
            this.Position = position;

            SyncLayers();
        }

        public override void Update()
        {
            for (int i = 0; i < _layers.Count; i++)
                _layers[i].Update();
        }

        public override void Render()
        {
            for (int i = 0; i < _layers.Count; i++)
                if (_layers[i].IsVisible)
                    _layers[i].Render();
        }

        public void SetLayerMetadata(int layer, Metadata data)
        {
            _layerMetadata[layer] = data;
        }

        public Metadata GetLayerMetadata(int layer)
        {
            return _layerMetadata[layer];
        }

        public void RemoveLayer(int layer)
        {
            _layers.RemoveAt(layer);
            _layerMetadata.RemoveAt(layer);
            SyncLayerIndex();
        }

        public CellsRenderer AddLayer(string name)
        {
            var layer = new CellsRenderer(new CellSurface(Width, Height), Batch);
            layer.Font = this.Font;
            _layers.Add(layer);
            _layerMetadata.Add(new Metadata() { Name = name, IsVisible = true });

            SyncLayerIndex();
            SyncLayers();
            return layer;
        }

        public void AddLayer(CellSurface surface)
        {
            var layer = new CellsRenderer(surface, Batch);
            layer.Font = this.Font;
            _layers.Add(layer);
            _layerMetadata.Add(new Metadata() { Name = "New", IsVisible = true });

            SyncLayerIndex();
            SyncLayers();
        }

        public CellsRenderer InsertLayer(int index)
        {
            var layer = new CellsRenderer(new CellSurface(Width, Height), Batch);
            layer.Font = this.Font;
            _layers.Insert(index, layer);
            _layerMetadata.Insert(index, new Metadata() { Name = index.ToString(), IsVisible = true });

            SyncLayerIndex();
            SyncLayers();
            return layer;
        }

        public void MoveLayer(int index, int newIndex)
        {
            var layer = _layers[index];
            var layerName = _layerMetadata[index];

            _layers.RemoveAt(index);
            _layerMetadata.RemoveAt(index);

            _layers.Insert(newIndex, layer);
            _layerMetadata.Insert(newIndex, layerName);

            SyncLayerIndex();
        }

        public IEnumerable<CellsRenderer> GetEnumeratorForLayers()
        {
            return _layers;
        }

        [OnDeserialized]
        private void AfterDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            SetActiveLayer(0);
        }
        private CellSurface _tempSurface;
        [OnSerializing]
        private void BeforeSerializing(StreamingContext context)
        {
            _tempSurface = _cellData;
            _cellData = null;
        }

        [OnSerialized]
        private void AfterSerialized(StreamingContext context)
        {
            _cellData = _tempSurface;
            _tempSurface = null;
        }

        public void ResizeCells(int width, int height   )
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].CellSize = new Point(width, height);
            }
        }

        private void SyncLayerIndex()
        {
            for (int i = 0; i < Layers; i++)
                _layerMetadata[i].Index = i;
        }
    }


}
