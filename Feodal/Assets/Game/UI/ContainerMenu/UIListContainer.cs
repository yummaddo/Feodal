using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Abstraction.UI;
using Game.Core.DataStructures.UI.Data;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Game.UI.ContainerMenu
{


    public class UIListContainer : MonoBehaviour
    {
        [SerializeField] Scroller scroller = default;
        [SerializeField] UIListContainerView view = default;
        [SerializeField] GameObject cellPrefab = default;
        [SerializeField] private List<UICellContainer> data;
        internal List<IUICellContainer> Data;
        internal Action<IUICellContainer> OnSelectionContainerChanged;
            
        void Start()
        {
            view.OnSelectionChanged(OnSelectionChanged);
            var items = Enumerable.Range(0, data.Count-1)
                .Select(i => new UICellContainerData(data[i].Data))
                .ToArray();
            view.UpdateData(items);
            view.SelectCell(0);
        }

        private void OnSelectionChanged(int obj)
        {
            
        }
    }
}