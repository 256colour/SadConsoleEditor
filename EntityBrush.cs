﻿using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadConsoleEditor
{
    class EntityBrush : Entity, IEntityBrush
    {
        public Animation TopLayerAnimation;

        public EntityBrush()
        {
            this.Font = Settings.ScreenFont;
        }

        //public override void Render()
        //{
        //    base.Render();

        //    var oldCellData = _cellData;
        //    _cellData = TopLayerAnimation.CurrentFrame;
        //    base.Render();

        //    _cellData = oldCellData;
        //}
    }
}