using System;
using Game.Core.Abstraction.UI;
using UnityEngine;

namespace Game.Core.DataStructures.UI.Data
{
    [CreateAssetMenu(menuName = "UI/UICellContainerElement")]
    public class UICellContainerElement : AbstractDataStructure<IUICellContainerElement>, IUICellContainerElement
    {
        public Sprite cellImage;
        public Sprite cellResource;
        public Sprite cellUniversalResource;

        public string cellTitle;
        public CellState state;
        public CellContainer container;
        public UICellContainer uIContainer;
        protected override IUICellContainerElement CompareTemplate()
        {
            CellImage = cellImage;
            CellTitle = cellTitle;
            State = state;
            Container = container;
            UIContainer = uIContainer;
            return this;
        }

        protected override string DataNamePattern => $"UICell_ContainerElement_{cellTitle}";

        public Sprite CellImage { get; set; }
        public string CellTitle { get; set; }
        public CellState State { get; set; }
        public CellContainer Container { get; set; }
        public UICellContainer UIContainer { get; set; }
    }
}