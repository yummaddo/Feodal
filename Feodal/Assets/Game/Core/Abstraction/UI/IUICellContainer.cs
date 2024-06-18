using System;
using System.Collections.Generic;
using Game.Core.DataStructures;
using Game.Core.DataStructures.UI;
using Game.Core.DataStructures.UI.Data;
using UnityEngine;

namespace Game.Core.Abstraction.UI
{
    public interface IUICellContainer
    {
        public Sprite CellImage { get; set; }
        public Sprite CellLendIdentImage { get; set; }
        public string CellTitle { get; set; }
        public GameObject CellTemplate { get; set; }
        public CellContainer Container { get; set; }
        public List<UICellContainerElement> UIContainer { get; set; }
    }
}