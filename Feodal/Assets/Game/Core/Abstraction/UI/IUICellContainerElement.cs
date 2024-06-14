using System;
using Game.Core.DataStructures;
using Game.Core.DataStructures.UI;
using Game.Core.DataStructures.UI.Data;
using UnityEngine;

namespace Game.Core.Abstraction.UI
{
    public interface IUICellContainerElement
    {
        public Sprite CellImage { get; set; }
        public string CellTitle { get; set; }
        public CellState State { get; set; }
        public CellContainer Container { get; set; }
        public UICellContainer UIContainer { get; set; }
    }
}