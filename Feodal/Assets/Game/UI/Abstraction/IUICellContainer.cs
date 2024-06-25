using System.Collections.Generic;
using Game.DataStructures;
using Game.DataStructures.UI;
using UnityEngine;

namespace Game.UI.Abstraction
{
    public interface IUICellContainer
    {
        public Seed Seed { get; set; }
        public Sprite CellImage { get; set; }
        public Sprite CellLendIdentImage { get; set; }
        public string CellTitle { get; set; }
        public GameObject CellTemplate { get; set; }
        public CellContainer Container { get; set; }
        public List<UICellContainerElement> UIContainer { get; set; }
    }
}