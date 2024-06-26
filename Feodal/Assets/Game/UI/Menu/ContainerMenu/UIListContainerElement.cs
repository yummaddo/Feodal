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
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(UpdateOnInit, SessionLifecycle.OnSceneStartServiceSession);
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
            try
            {
                callBack.onClick.AddListener(OnButtonClick);
                _seedControllingMicroservice = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellSeedControllingMicroservice>();
                UICellContainerProvider.CallBackTunneling<UIMenuBuilding>(this);
            }
            catch (Exception e)
            {
                Debugger.Logger(e.Message, Process.TrashHold);
            }
        }

        private void OnEnable()
        {
            if (!_isInit)
            {
                UpdateOnInit();
            }
            else
            {
                UpdatePosition(_currentPosition);
                try
                {
                    if (_seedControllingMicroservice && uIContainer != null)
                        pool.color = _seedControllingMicroservice.CanCreateNewSeed(uIContainer.Data.Container.seed) ? availableColor : unAvailableColor;
                }
                catch (Exception e)
                {
                    Debugger.Logger(e.Message, Process.TrashHold);
                }
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
                    pool.color = _seedControllingMicroservice.CanCreateNewSeed(itemContainerData.Data.Container.seed) ? availableColor : unAvailableColor;
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