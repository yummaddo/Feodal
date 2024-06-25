using Game.DataStructures;
using Game.DataStructures.UI;
using UnityEngine;

namespace Game.UI.Abstraction
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