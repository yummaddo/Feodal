using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.Cells;
using Game.Services.Abstraction;
using Game.Services.CellServices;
using Game.Services.ControlServices;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu;
using Game.Utility;
using UnityEngine;

namespace Game.Services.InputServices.Microservice
{
    public class InputObservingMicroservice : AbstractMicroservice<InputService>
    {        
        [Header("Program input parameters")]
        [Range(0.3f, 0.01f)]public float timeForClick = 0.02f;
        [Range(0.31f, 1.5f)]public float timeForSelection = 0.5f;
        [Range(0.01f, 1f)]public float speedForCameraMove = 0.02f;
        [Range(10f, 200)]public float maximumTouchScreenDistance = 10f;

        [Header("Program speed parameters")] 
        public AnimationCurve elevation;
        [Range(0.5f, 100f)] public float cameraMovementSpeed = 2f;
        
        private Vector2 _screenPositionStart;
        private Vector2 _screenPositionProcessed;
        private Vector2 _screenPositionProcessedLast;
        
        private bool _isMoveCamera = false;
        private float _time = 0;
        private bool _isClick = false;
        private bool _activeTouch = false;
        internal bool ActiveStatus => _technology && _tradeMenu && _buildingMenu && _containerMenu && _initMap && _cellSetterMenu;

        private bool _initMap = false;
        private bool _technology = true;
        private bool _tradeMenu = true;
        private bool _buildingMenu = true;
        private bool _containerMenu = true;        
        private bool _cellSetterMenu = true;
        
        private float _screenSwipeSpeed = 0;
        private float _distanceOfStartEndTouch = 0;
        
        private ControlService _controlService;
        private CellService _cellService;
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
        protected override Task OnAwake(IProgress<float> progress)
        {
            Proxy.Connect<CellProvider, Cell>(OpenMenuCallFormMenu,  Port.SelectionCellBase);
            Proxy.Connect<CellProvider, Cell>(ExitMenuCallFormMenu,  Port.CellDeleteBuilding);
            Proxy.Connect<CellProvider, Cell>(ExitMenuCallFormMenu,  Port.CellDeleteContainer);
            Proxy.Connect<CellAddDetectorProvider, CellAddDetector, CellAddDetector>(OpenMenuCallFormMenu);
            // all exit
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>(ExitMenuCallFormMenu);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, UIMenuBuilding>(ExitMenuCallFormMenu);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, UIMenuContainer>(ExitMenuCallFormMenu);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, UITechnologyMenu>(ExitMenuCallFormMenu);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, UITradeMenu>(ExitMenuCallFormMenu);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, UICellSetterMenu>(ExitMenuCallFormMenu);
            // all open
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, ButtonOpenMenuCallBack>(OpenMenuCallFormMenu);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, UIMenuBuilding>(OpenMenuCallFormMenu);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, UIMenuContainer>(OpenMenuCallFormMenu);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, UITechnologyMenu>(OpenMenuCallFormMenu);
            Proxy.Connect<MenuTypesOpenProvider, MenuTypes, UITradeMenu>(OpenMenuCallFormMenu);
            //
            _controlService = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<ControlService>();
            SessionLifeStyleManager.AddLifeIteration(InstanceSceneSession,SessionLifecycle.OnSceneAwakeClose);
            return Task.CompletedTask;
        }

        private Task InstanceSceneSession(IProgress<float> progress)
        {
            _controlService = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<ControlService>();
            _cellService = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellService>();
            if (_cellService.IsMapCellInitial) 
                _initMap = true;
            else 
                _cellService.OnCellMapInitial += CellServiceCellMapInitial;
            return Task.CompletedTask;
        }
        protected override Task OnStart(IProgress<float> progress) { return Task.CompletedTask;}

        private void CellServiceCellMapInitial(CellMap obj) => _initMap = true;
        private void ExitMenuCallFormMenu(Port arg1, Cell arg2) =>  MenuCallFormMenuSwitcher(MenuTypes.CellSetterMenu, true);
        private void OpenMenuCallFormMenu(Port arg1, Cell arg2) => MenuCallFormMenuSwitcher(MenuTypes.CellSetterMenu, false);
        private void OpenMenuCallFormMenu(Port arg1, MenuTypes arg2) => MenuCallFormMenuSwitcher(arg2, false);
        private void ExitMenuCallFormMenu(Port arg1, MenuTypes arg2) => MenuCallFormMenuSwitcher(arg2, true);
        private void OpenMenuCallFormMenu(Port type, CellAddDetector obj) => _containerMenu = false;
        
        private void MenuCallFormMenuSwitcher(MenuTypes menu, bool status)
        {
            if (menu == MenuTypes.Technology)
                _technology = status;
            if (menu == MenuTypes.TradeMenu)
                _tradeMenu = status;
            if (menu == MenuTypes.ContainerMenu)
                _containerMenu = status;
            if (menu == MenuTypes.BuildingMenu)
                _buildingMenu = status;
            if (menu == MenuTypes.CellSetterMenu)
                _cellSetterMenu = status;
        }
        private void Update()
        {
            if (!ActiveStatus) return;
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
            else _activeTouch = false;
            if ((_distanceOfStartEndTouch > maximumTouchScreenDistance) && _isClick) { _isMoveCamera = true; }
        }

        private void LateUpdate()
        {
            if (ActiveStatus && !_activeTouch && _isClick) // find touch result
            {
                if (_time > timeForClick && _time >= timeForSelection && !_isMoveCamera) // selection
                    OnSelectDetector();
                else if (_time < timeForClick && !_isMoveCamera) // click
                    OnClickDetector();
            } 
            else if (ActiveStatus && _activeTouch && _isClick && _isMoveCamera) // camera move condition
            {
                var vector2Normal = _screenPositionProcessed - _screenPositionProcessedLast;
                var magnitudeDivision = vector2Normal.magnitude * cameraMovementSpeed;
                Mathf.Clamp(magnitudeDivision, 0.001f, 5f);
                var vector3Normal = new Vector3(-vector2Normal.y, 0, vector2Normal.x).normalized * magnitudeDivision ;
                vector3Normal = Vector3.ClampMagnitude(vector3Normal * Time.deltaTime, speedForCameraMove); 
                _controlService.MoveCamera(vector3Normal ,  vector3Normal.magnitude, vector3Normal);
            }

            if (!_activeTouch && _isClick) Reset();
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
            CheckRaycastHitDetector<CellAddDetector>("AddCell",false);
            CheckRaycastHitDetector<CellUpdatedDetector>("Cell",false);
        }
        private void OnClickDetector()
        {
            CheckRaycastHitDetector<CellAddDetector>("AddCell",true);
            CheckRaycastHitDetector<CellUpdatedDetector>("Cell",true);
        }
        private void CheckRaycastHitDetector<TType>(string teg,bool isClick = true)
            where TType : IDetector
        {
            Ray ray = Service.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
                if (hit.collider.CompareTag(teg))
                {
                    var obj = hit.collider.gameObject;
                    var componentInParent = obj.GetComponentInParent<TType>();
                    if (componentInParent != null)
                    {
                        if (isClick)
                            componentInParent.OnClick();
                        else
                            componentInParent.OnSelect();
                    }
                }
        }
    }
}