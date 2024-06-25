using System;
using System.Collections.Generic;
using Game.DataStructures.UI;
using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI.Extensions.EasingCore;

namespace Game.UI.Menu.ContainerMenu
{


    public class UIListContainerView : FancyScrollView<UICellContainerData,Context>
    {
        [SerializeField] Scroller scroller = default;
        [SerializeField] GameObject cellPrefab = default;
        protected override GameObject CellPrefab => cellPrefab;
        Action<int> onSelectionChanged;

        protected override void Initialize()
        {
            base.Initialize();

            Context.OnCellClicked = SelectCell;

            scroller.OnValueChanged(UpdatePosition);
            scroller.OnSelectionChanged(UpdateSelection);
        }

        void UpdateSelection(int index)
        {
            if (Context.SelectedIndex == index)
            {
                return;
            }

            Context.SelectedIndex = index;
            Refresh();
        }

        public void UpdateData(IList<UICellContainerData> items)
        {
            UpdateContents(items);
            scroller.SetTotalCount(items.Count);
        }

        public void SelectCell(int index)
        {
            if (index < 0 || index >= ItemsSource.Count || index == Context.SelectedIndex)
            {
                return;
            }

            UpdateSelection(index);
            scroller.ScrollTo(index, 0.35f, Ease.OutCubic);
        }
        
        public void OnSelectionChanged(Action<int> callback) => onSelectionChanged = callback;
    }
}