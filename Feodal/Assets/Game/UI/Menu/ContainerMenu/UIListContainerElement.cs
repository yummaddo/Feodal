using System;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.CallBacks.CallbackClick.Simple;
using Game.DataStructures;
using Game.DataStructures.UI;
using Game.Services.CellServices.Microservice;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Services.ProxyServices.Providers;
using Game.UI.Abstraction;
using Game.Utility;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Game.UI.Menu.ContainerMenu
{

    public class UIListContainerElement : FancyCell<UICellContainerData, Context>
    {
        public UICellContainer uIContainer;
        public CellContainer container;
        public Button callBack;
        public SimpleMenuTypesCloseCallBack closeMenuCallBack;
        public MenuTypes menuTypesToClose = MenuTypes.ContainerMenu;
        public Animator animator;
        [Header("Style")]
        public Color availableCellColor = Color.white;
        public Color availableColor = Color.green;
        public Color unAvailableColor = Color.gray;
        [Header("Controlling")]
        public Image seed;
        public Image cell;
        public Image pool;
        public Text title;
        public Action<Port, IUICellContainer> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; }
        private static readonly int Scroll = Animator.StringToHash("scroll");
        private float _currentPosition = 0;
        private CellSeedControllingMicroservice _seedControllingMicroservice;
        private IUICellContainer _uiContainer;
        
        private bool _isInit = false;
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(UpdateOnInit, SessionLifecycle.OnSceneStartServiceSession);
            SessionLifeStyleManager.AddLifeIteration(UpdateDependency, SessionLifecycle.OnSceneAwakeClose);
        }
        private void OnEnable()
        {
            if (!_isInit)
            {
                UpdateOnInit();
                OnEnableSProcess();
            }
            else
                OnEnableSProcess();
        }
        private Task UpdateDependency(IProgress<float> arg)
        {
            return Task.CompletedTask;
        }
        private Task UpdateOnInit(IProgress<float> progress)
        {
            _isInit = true;
            UpdateOnInit();
            return Task.CompletedTask;
        }
        private void UpdateOnInit() 
        {
            _isInit = true;
            callBack.onClick.AddListener(OnButtonClick);
            _seedControllingMicroservice = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellSeedControllingMicroservice>();
        }
        private void OnEnableSProcess()
        {
            if (!_isInit) 
                UpdateOnInit();
            else
            {
                try
                {
                    UpdatePosition(_currentPosition);
                    TryToColorUpdate();
                }
                catch (Exception e)
                {
                    Debugger.Logger(e.Message, Process.TrashHold);
                }
            }
        }

        private void TryToColorUpdate()
        {
            if (SessionLifeStyleManager.Instance.IsMicroServiceSessionInit && uIContainer != null)
            {
                _seedControllingMicroservice = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellSeedControllingMicroservice>();
                UpdateCellContainerColor();
            }
        }

        private void UpdateCellContainerColor()
        {
            if (_seedControllingMicroservice.CanCreateNewSeed(uIContainer.Data.Container.seed))
            {
                pool.color = availableColor;
                cell.color = availableCellColor;
            }
            else
            {
                pool.color = unAvailableColor;
                cell.color = unAvailableColor;
            }
        }

        private void OnButtonClick()
        {
            try
            {
                // UICellContainerProvider.CallBackTunneling<UIMenuBuilding>(this);
                _seedControllingMicroservice = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellSeedControllingMicroservice>();
                if (_seedControllingMicroservice.CanCreateNewSeed(uIContainer.Data.Container.seed))
                {
                    Debugger.Logger($"Create {uIContainer.Data.Container.seed}", Process.Create);
                    _seedControllingMicroservice.ContainerClick(uIContainer.Data);
                }
                else 
                    Debugger.Logger($"No Amount {uIContainer.Data.Container.seed}");
            }
            catch (Exception e)
            {
                Debugger.Logger(e.ToString(), Process.TrashHold);
            }

        }
        public override void UpdateContent(UICellContainerData itemContainerData)
        {
            try
            {
                _uiContainer = itemContainerData.Data;
                uIContainer = itemContainerData.Data;
                container = itemContainerData.Data.container;
                cell.sprite = _uiContainer.CellImage;
                seed.sprite = _uiContainer.CellLendIdentImage;
                title.text = container.containerName;
                if (_seedControllingMicroservice) 
                    UpdateCellContainerColor();
            }
            catch (Exception e)
            {
                Debugger.Logger(e.ToString(), Process.TrashHold);
            }
        }
        public override void UpdatePosition(float position)
        {
            _currentPosition = position;
            if (animator.isActiveAndEnabled)
                animator.Play(Scroll, -1, position);
            animator.speed = 0;
        }
    }
}