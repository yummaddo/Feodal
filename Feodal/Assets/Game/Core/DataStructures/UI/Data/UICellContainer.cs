using System;
using System.Collections.Generic;
using Game.Core.Abstraction.UI;
using UnityEngine;

namespace Game.Core.DataStructures.UI.Data
{
    [CreateAssetMenu(menuName = "UI/UICellContainer")]
    public class UICellContainer : AbstractDataStructure<IUICellContainer>, IUICellContainer
    {
        public Sprite cellImage;
        public Sprite cellLendIdentImage;
        public String cellTitle;
        public CellContainer container;
        public List<UICellContainerElement> uIContainer;
        public GameObject cellTemplate;

        protected override string DataNamePattern => $"UICell_Container_{cellTitle}";
        public Sprite CellImage { get; set; }
        public Sprite CellLendIdentImage { get; set; }
        public string CellTitle { get; set; }
        public GameObject CellTemplate { get; set; }
        public CellContainer Container { get; set; }
        public List<UICellContainerElement> UIContainer { get; set; }

        protected override IUICellContainer CompareTemplate()
        {
            UIContainer = uIContainer;
            CellLendIdentImage = cellLendIdentImage;
            CellImage = cellImage;
            CellTitle = cellTitle;
            Container = container;
            CellTemplate = cellTemplate;
            return this;
        }
    }
}