using System;
using System.Collections.Generic;
using Game.Core.Detector;
using Game.Meta;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.ClickCallback.Abstraction;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core.Cells
{
    public class CellUpdateDetector : MonoBehaviour
    {
        public DetectorIndexing detectorIndexing;
        private SessionStateManager _manager;
        public Cell cell;
        public SimpleClickCallback<Cell> clickCallback;
        public SimpleClickCallback<Cell> clickSelectedCallback;
        public void OnSelect()
        {
        }
        public void OnClick()
        {
        }
    }
}