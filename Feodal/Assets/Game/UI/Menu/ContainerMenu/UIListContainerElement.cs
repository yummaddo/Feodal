using System;
using Game.Core.Abstraction.UI;
using Game.Core.DataStructures;
using Game.Core.DataStructures.UI;
using Game.Core.DataStructures.UI.Data;
using Game.Meta;
using Game.Services.CellControlling.Microservice;
using Game.Services.Proxies;
using Game.Services.Proxies.Abstraction;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.ClickCallback.Simple;
using Game.Services.Proxies.Providers;
using Game.Services.Storage;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Game.UI.Menu.ContainerMenu
{

    public class UIListContainerElement : FancyCell<UICellContainerData, Context>, ICallBack<IUICellContainer>
    {
        public UICellContainer uIContainer;
        public CellContainer Container;

        public Button callBack;
        public SimpleMenuTypesCloseCallBack closeMenuCallBack;
        public MenuTypes menuTypesToClose = MenuTypes.ContainerMenu;
        public Animator animator;
        public AnimationCurve x;
        public Color availableColor = Color.green;
        public Color unAvailableColor = Color.gray;
        public Image seed;
        public Image cell;
        
        public Image pool;
        
        public Text title;
        public Text price;
        
        public Action<Port, IUICellContainer> OnCallBackInvocation { get; set; }
        public bool IsInit { get; set; }
        
        private static readonly int Scroll = Animator.StringToHash("scroll");
        private float _currentPosition = 0;
        private CellSeedControllingMicroservice _seedControllingMicroservice;
        
        internal IUICellContainer UIContainer;
        
        private bool _isInit = false;
        private void OnEnable()
        {
            var sessionManager = SessionStateManager.Instance;
            if (sessionManager.IsMicroServiceSessionInit && !_isInit)
            {
                UpdateOnInit();
            }
            else if (!_isInit)
            {
                sessionManager.OnSceneStartSession += UpdateOnInit;
            }

            UpdatePosition(_currentPosition);
            try
            {
                if (_seedControllingMicroservice && uIContainer != null)
                {
                    pool.color = _seedControllingMicroservice.CanCreateNewSeed(uIContainer.Data.Container.seed) ? availableColor : unAvailableColor;
                }
            }
            catch (Exception e)
            {
                Debugger.Logger(e.Message, Process.TrashHold);
            }
        }

        private void UpdateOnInit()
        {
            _isInit = true;
            try
            {
                callBack.onClick.AddListener(OnButtonClick);
                _seedControllingMicroservice = SessionStateManager.Instance.ServiceLocator.Resolve<CellSeedControllingMicroservice>();
                UICellContainerProvider.CallBackTunneling<UIMenuBuilding>(this);
            }
            catch (Exception e)
            {
                Debugger.Logger(e.Message, Process.TrashHold);
            }
        }
        private void OnButtonClick()
        {
            if (_seedControllingMicroservice.CanCreateNewSeed(uIContainer.Data.Container.seed))
            {
                Debugger.Logger($"Create {uIContainer.Data.Container.seed}", Process.Create);
                OnCallBackInvocation?.Invoke(Porting.Type<UIMenuBuilding>(),uIContainer.Data);
                closeMenuCallBack.OnCallBackInvocation?.Invoke(Porting.Type<ButtonExitMenuCallBack>(), menuTypesToClose);
            }
            else
            {
                Debugger.Logger($"No Amount {uIContainer.Data.Container.seed}");

            }
        }
        public override void UpdateContent(UICellContainerData itemContainerData)
        {
            try
            {
                UIContainer = itemContainerData.Data;
                uIContainer = itemContainerData.Data;
                Container = itemContainerData.Data.container;
                cell.sprite = UIContainer.CellImage;
                seed.sprite = UIContainer.CellLendIdentImage;
                title.text = UIContainer.Container.containerName;
                price.text = UIContainer.Container.price.ToString();
                if (_seedControllingMicroservice)
                {
                    pool.color = _seedControllingMicroservice.CanCreateNewSeed(itemContainerData.Data.Container.seed) ? availableColor : unAvailableColor;
                }
            }
            catch (Exception e)
            {
                Debugger.Logger(e.Message, Process.TrashHold);
            }

        }
        public override void UpdatePosition(float position)
        {
            _currentPosition = position;
            if (animator.isActiveAndEnabled)
            {
                animator.Play(Scroll, -1, position);
            }
            animator.speed = 0;
        }
    }
}