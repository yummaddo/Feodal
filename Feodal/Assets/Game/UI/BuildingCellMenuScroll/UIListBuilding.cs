using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Abstraction.UI;
using Game.Core.DataStructures.UI.Data;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Game.UI.BuildingCellMenuScroll
{
    public class UIListBuildingInContainer : MonoBehaviour
    {
        [SerializeField] Scroller scroller = default;
        [SerializeField] UIListBuildingView view = default;
        [SerializeField] GameObject cellPrefab = default;
        [SerializeField] private UICellContainer data;
        internal List<IUICellContainerElement> Data;
        internal Action<IUICellContainerElement> OnSelectionContainerChanged;
        
        void Start()
        {
            view.OnSelectionChanged(OnSelectionChanged);
            var items = Enumerable.Range(0, data.container.states.Count-1)
                .Select(i => new UICellBuildingData(data.uIContainer[i].Data))
                .ToArray();
            view.UpdateData(items);
            view.SelectCell(0);
        }
        
        private void OnSelectionChanged(int obj)
        {
            
        }
    }
}