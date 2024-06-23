using System;
using System.Collections.Generic;
using Game.Core.Cells;
using Game.Meta;
using Game.Services.Abstraction.MicroService;
using Game.Services.Control;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;
using UnityEngine;

namespace Game.Services.Inputs.Microservice
{
    public class InputObservingMicroservice : AbstractMicroservice<InputService>
    {        
        [Header("Program input parameters")]
        [Range(0.3f, 0.01f)]public float timeForClick = 0.02f;
        [Range(0.31f, 1.5f)]public float timeForSelection = 0.5f;
        [Range(0.01f, 0.1f)]public float speedForCameraMove = 0.02f;
        [Range(10f, 200)]public float maximumTouchScreenDistance = 10f;
        [Header("Program speed parameters")] 
        [Range(0.5f, 10f)] public float cameraMovementSpeed = 2f;
        private Vector2 _screenPositionStart;
        private Vector2 _screenPositionProcessed;
        private Vector2 _screenPositionProcessedLast;
        
        private bool _isMoveCamera = false;
        private float _time = 0;
        private bool _isClick = false;
        private bool _activeTouch = false;
        private bool _active = true;
        
        private float _screenSwipeSpeed = 0;
        private float _distanceOfStartEndTouch = 0;
        
        private ControlService _controlService;
        private Dictionary<int, CellAddDetector> _detectors;
        private Dictionary<int, CellUpdatedDetector> _cellsUpdateDetectors;

#if UNITY_EDITOR
        private List<CellAddDetector> _detectorsList;
        private List<CellUpdatedDetector> _cellsUpdateDetectorsList;
        private List<int> _detectorsListID;
        private List<int> _cellsUpdateDetectorsListID;
#endif

        protected override void Awake()
        {
            base.Awake();
            _detectors = new Dictionary<int, CellAddDetector>();
            _cellsUpdateDetectors = new Dictionary<int, CellUpdatedDetector>();

#if UNITY_EDITOR
            
            _detectorsList = new List<CellAddDetector>();
            _cellsUpdateDetectorsList = new List<CellUpdatedDetector>();
            _detectorsListID = new List<int>();
            _cellsUpdateDetectorsListID = new List<int>();
#endif
        }
        protected override void OnAwake()
        {
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>(ExitMenuCall);
            Proxy.Connect<CellAddDetectorProvider, CellAddDetector, CellAddDetector>(OpenMenuCall);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>(ExitMenuCallFormMenu);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, ButtonOpenMenuCallBack>(OpenMenuCallFormMenu);
            _controlService = SessionStateManager.Instance.ServiceLocator.Resolve<ControlService>();
        }

        private void OpenMenuCallFormMenu(Port arg1, MenuTypes arg2)
        {
            if (arg2 == MenuTypes.Technology || arg2 == MenuTypes.TradeMenu)
            {
                _active = false;
            }
        }
        private void ExitMenuCallFormMenu(Port arg1, MenuTypes arg2)
        {
            if (arg2 == MenuTypes.Technology)
            {
                _active = true;
            }
        }
        private void OpenMenuCall(Port type, CellAddDetector obj)
        {
            _active = false;
        }
        
        private void ExitMenuCall(Port type, MenuTypes obj)
        {
            _active = true;
        }
        public void AddDetector(CellAddDetector detector)
        {
#if UNITY_EDITOR
            _detectorsList.Add(detector);
            _detectorsListID.Add(detector.GetInstanceID());
#endif
            foreach (var objectWithCollider in detector.objectsWithCollider)
                _detectors.Add(objectWithCollider.GetInstanceID(), detector);
        }

        public void DeleteDetector( List<GameObject> detector)
        {
#if UNITY_EDITOR
            List<CellAddDetector> forClear = new List<CellAddDetector>();
            foreach (var cell in _detectorsList) if (cell == null) forClear.Add(cell);
            foreach (var clear in forClear) _detectorsList.Remove(clear);
            foreach (var colliderObject in detector) _detectorsListID.Remove(colliderObject.GetInstanceID());
#endif
            try
            {
                foreach (var objectWithCollider in detector)
                    _detectors.Remove(objectWithCollider.GetInstanceID());
            }
            catch (Exception e) { Debugger.Logger(e.Message, Process.TrashHold); }
        }

        public void AddDetector(CellUpdatedDetector detector)
        {
#if UNITY_EDITOR
            _cellsUpdateDetectorsList.Add(detector);
            _cellsUpdateDetectorsListID.Add(detector.GetInstanceID());
#endif
            foreach (var objectWithCollider in detector.objectsWithCollider)
                _cellsUpdateDetectors.Add(objectWithCollider.GetInstanceID(), detector);
        }
        public void DeleteUpdateDetectors(List<GameObject> detectors)
        {
#if UNITY_EDITOR
            List<CellUpdatedDetector> forClear = new List<CellUpdatedDetector>();
            foreach (var cell in _cellsUpdateDetectorsList) if (cell == null) forClear.Add(cell);
            foreach (var cell in forClear) _cellsUpdateDetectorsList.Remove(cell);
            foreach (var colliderObject in detectors) _cellsUpdateDetectorsListID.Remove(colliderObject.GetInstanceID());
#endif
            try
            {
                foreach (var objectWithCollider in detectors)
                {
                    _cellsUpdateDetectors.Remove(objectWithCollider.GetInstanceID());
                }
            }
            catch (Exception e) { Debugger.Logger(e.Message, Process.TrashHold); }
        }
        
        protected override void OnStart() { }
        protected override void Stop() { }
        protected override void ReStart() { }

        private void Update()
        {
            if (!_active) return;
            if (Input.GetButton("Fire1"))
            {
                _activeTouch = true;
                if (!_isClick)
                {
                    _screenPositionStart = Input.mousePosition;
                    _screenPositionProcessed = Input.mousePosition;
                    _screenPositionProcessedLast = _screenPositionProcessed;
                    _isClick = true;
                    _time = 0;
                }
                else
                {
                    var temp = Input.mousePosition;
                    _distanceOfStartEndTouch = Vector2.Distance(_screenPositionStart, temp);
                    _screenPositionProcessedLast = _screenPositionProcessed;
                    _screenPositionProcessed = temp;
                    _screenSwipeSpeed = _distanceOfStartEndTouch / Time.deltaTime;
                }
                
                _time += Time.deltaTime;
            }
            else
            {
                _activeTouch = false;
            }
            if ((_distanceOfStartEndTouch > maximumTouchScreenDistance) && _isClick) { _isMoveCamera = true; }
        }

        private void LateUpdate()
        {
            if (_active && !_activeTouch && _isClick) // find touch result
            {
                if (_time > timeForClick && _time >= timeForSelection && !_isMoveCamera) // selection
                    OnSelectDetector();
                else if (_time < timeForClick && !_isMoveCamera) // click
                    OnClickDetector();
            } 
            else if (_active && _activeTouch && _isClick && _isMoveCamera) // camera move condition
            {
                var distanceFromStart = _screenPositionProcessed- _screenPositionStart;
                _controlService.MoveCamera(cameraMovementSpeed, _screenPositionProcessedLast, _screenPositionProcessed, distanceFromStart);
            }

            if (!_activeTouch && _isClick)
            {
                Reset();
            }
        }

        private void Reset()
        {
            _screenPositionStart = Vector2.zero;
            _screenPositionProcessed = Vector2.zero;
            _screenPositionProcessedLast = Vector2.zero;
            _isMoveCamera = false;
            _time = 0;
            _isClick = false;
            _activeTouch = false;
            _screenSwipeSpeed = 0;
            _distanceOfStartEndTouch = 0;
        }

        private void OnSelectDetector()
        {
            CheckRaycastHitAddCellDetector((int index) => { _detectors[index].OnSelect();});
            CheckRaycastHitCellDetector((int index) => { _cellsUpdateDetectors[index].OnSelect();});
        }
        
        private void OnClickDetector()
        {
            CheckRaycastHitAddCellDetector((int index) => { _detectors[index].OnClick();});
            CheckRaycastHitCellDetector((int index) => { _cellsUpdateDetectors[index].OnClick();});
        }
        
        private void CheckRaycastHitAddCellDetector(Action<int> action)
        {
            Ray ray = Service.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
                if (hit.collider.CompareTag("AddCell"))
                {
                    var obj = hit.collider.gameObject.GetInstanceID();
                    if (_detectors.ContainsKey(obj))
                    {
                        action(obj);
                    }
                }
        }
        
        private void CheckRaycastHitCellDetector(Action<int> action)
        {
            Ray ray = Service.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
                if (hit.collider.CompareTag("Cell"))
                {
                    var obj = hit.collider.gameObject.GetInstanceID();
                    if (_cellsUpdateDetectors.ContainsKey(obj))
                    {
                        action(obj);
                    }
                }
        }

    }
}