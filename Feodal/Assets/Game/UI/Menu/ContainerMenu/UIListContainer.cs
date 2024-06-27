using System;
using System.Collections.Generic;
using System.Linq;
using Game.DataStructures.UI;
using Game.Services.ProxyServices.Providers;
using Game.UI.Abstraction;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Game.UI.Menu.ContainerMenu
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
            var items = Enumerable.Range(0, data.Count)
                .Select(i => new UICellContainerData(data[i]))
                .ToArray();
            view.UpdateData(items);
            view.SelectCell(0);
        }

        private void OnSelectionChanged(int obj)
        {
            
        }
    }
}