using System;
using System.Collections.Generic;
using Game.Core.Cells;
using Game.Meta;
using Game.Services.Abstraction.MicroService;
using Game.Services.Control;
using Game.Services.Proxies;
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
        [Range(10f, 100f)]public float maximumTouchScreenDistance = 10f;
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
        private Dictionary<int, CellUpdateDetector> _cellsUpdateDetectors;
        
        protected override void Awake()
        {
            base.Awake();
            _detectors = new Dictionary<int, CellAddDetector>();
            _cellsUpdateDetectors = new Dictionary<int, CellUpdateDetector>();
        }
        protected override void OnAwake()
        {
            Proxy.Connect<MenuExitProvider, MenuTypes>(ExitMenuCall);
            _controlService = SessionStateManager.Instance.Container.Resolve<ControlService>();
        }
        
        public void AddDetector(CellAddDetector detector) => _detectors.Add(detector.GetInstanceID(), detector);
        public void DeleteDetector(CellAddDetector detector)
        {
            try { _detectors.Remove(detector.GetInstanceID()); }
            catch (Exception e) { Debugger.Logger(e.Message, Process.TrashHold); }
        }
        public void AddDetector(CellUpdateDetector detector) => _cellsUpdateDetectors.Add(detector.GetInstanceID(), detector);
        public void DeleteDetector(CellUpdateDetector detector)
        {
            try { _cellsUpdateDetectors.Remove(detector.GetInstanceID()); }
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
                    // _screenPositionStart = Input.mousePosition;
                    _screenPositionProcessed = Input.mousePosition;
                    _screenPositionProcessedLast = _screenPositionProcessed;
                    _isClick = true;
                    _time = 0;
                }
                else
                {
                    var temp = Input.mousePosition;
                    _distanceOfStartEndTouch = Vector3.Distance(temp, _screenPositionProcessed);
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
            if (_distanceOfStartEndTouch > maximumTouchScreenDistance && _isClick) { _isMoveCamera = true; }
        }

        private void LateUpdate()
        {
            if (_active && !_activeTouch && _isClick) // find touch result
            {
                if (_time > timeForClick && _time >= timeForSelection && !_isMoveCamera) // selection
                    OnSelectDetector();
                else if (_time < timeForClick && !_isMoveCamera) // click
                    OnClickDetector();
                Reset(); // reset after run clicked end callback
            } 
            else if (_active && _activeTouch && _isClick && _isMoveCamera) // camera move condition
            {
                _controlService.MoveCamera(cameraMovementSpeed, _screenPositionProcessedLast, _screenPositionProcessed);
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
            _active = true;
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
        
        private bool RayCastToUI()
        {
            var myObjectLayer = LayerMask.NameToLayer("UI");
            int layerMask = 1 << myObjectLayer;
            Ray ray = Service.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100.0f, layerMask))
                if (hit.point != Vector3.zero) return true;
            return false;
        }
        
        private void CheckRaycastHitAddCellDetector(Action<int> action)
        {
            Ray ray = Service.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
                if (hit.collider.CompareTag("AddCell"))
                {
                    var obj = hit.collider.gameObject.GetInstanceID();
                    if (_detectors.ContainsKey(obj)) action(obj);
                }
        }
        
        private void CheckRaycastHitCellDetector(Action<int> action)
        {
            Ray ray = Service.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
                if (hit.collider.CompareTag("Cell"))
                {
                    var obj = hit.collider.gameObject.GetInstanceID();
                    if (_cellsUpdateDetectors.ContainsKey(obj)) action(obj);
                }
        }
        
        private void ExitMenuCall(MenuTypes obj)
        {
            _active = true;
        }
    }
}