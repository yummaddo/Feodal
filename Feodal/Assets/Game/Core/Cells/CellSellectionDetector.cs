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
        private bool _isClick = false;
        private float _time = 0.0f;
        private bool _active = true;
        public DetectorIndexing detectorIndexing;
        [Range(0.4f,2f)][SerializeField]
        private float selectionTime = 0.5f;
        [Range(0.1f,2f)][SerializeField]
        private float clickTime = 0.5f;
        private SessionStateManager _manager;
        public Cell cell;
        public  SimpleClickCallback<Cell> clickCallback;
        public  SimpleClickCallback<Cell> clickSelectedCallback;
        void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                if (detectorIndexing.CheckRaycastHit("Cell", detectorIndexing) && !_isClick) { _isClick = true; }
                _time += Time.deltaTime;
            }
            else
            {
                if (_isClick)
                {
                    _time += Time.deltaTime;
                        if (_time >= selectionTime)
                            clickCallback.OnClick?.Invoke(cell);
                        if (_time >= selectionTime)
                            clickSelectedCallback.OnClick?.Invoke(cell);
                }
                _time = 0;
                _isClick = false;
            }
        }
    }
}