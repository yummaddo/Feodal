using System;
using Game.Core.Detector;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;
using UnityEngine;

namespace Game.Core.Cells
{
    public class CellAddDetector : MonoBehaviour
    {
        private bool _active = true;
        private bool _isClick = false;
        private float _time = 0.0f;
        public DetectorIndexing detectorIndexing;
        private SessionStateManager _manager;
        public SimpleClickCallback<CellAddDetector> clickCallback;

        private void Awake()
        {
            Proxy.Connect<CellAddProvider,CellAddDetector>(OnSelectDetector);
            Proxy.Connect<MenuExitProvider, MenuTypes>(ExitMenuCall);
        }
        private void ExitMenuCall(MenuTypes obj)
        {
            _active = true;
        }
        private void OnSelectDetector(CellAddDetector obj)
        {
            _active = false;
        }
        void Update()
        {
            if (!_active) return;
            if (Input.GetButton("Fire1"))
            {
                if (detectorIndexing.CheckRaycastHit("AddCell",detectorIndexing)  && !_isClick)
                {
                    _isClick = true;
                    _time = 0;
                }
                _time += Time.deltaTime;

            }
            else
            {
                if (_isClick)
                {
                    _time += Time.deltaTime;
                    if (detectorIndexing.CheckRaycastHit("AddCell", detectorIndexing))
                    {
                        clickCallback.OnClick?.Invoke(this);
                    }
                }
                _time = 0;
                _isClick = false;
            }
        }
    }
}