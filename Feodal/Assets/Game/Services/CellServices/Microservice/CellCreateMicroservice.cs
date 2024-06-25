using System;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.Cells;
using Game.Services.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.UI.Abstraction;
using Game.UI.Menu;
using UnityEngine;

namespace Game.Services.CellServices.Microservice
{
    public class CellCreateMicroservice : AbstractMicroservice<CellService>
    {
        private CellService _service;
        public bool createActive = false;
        public CellAddDetector cellAddDetector  = null;
        public GameObject template;
        protected override Task OnAwake(IProgress<float> progress)
        {
            _service = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellService>();
            return Task.CompletedTask;
        }
        protected override Task OnStart(IProgress<float> progress)
        {
            Proxy.Connect<CellAddDetectorProvider, CellAddDetector,CellAddDetector>( ClickedByAddCellObject);
            Proxy.Connect<UICellContainerProvider, IUICellContainer,UIMenuContainer>( ClickedByUICellContainerObject);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>( ClickedByUICellContainerExit);
            return Task.CompletedTask;
        }

        private void ClickedByAddCellObject(Port type,CellAddDetector detector)
        {
            cellAddDetector = detector;
            createActive = true;
            _service.PauseTheAddCellElements();
        }
        
        private void ClickedByUICellContainerObject(Port type,IUICellContainer container)
        {
            _service.TyAddCell(cellAddDetector, container);
        }
        
        private void ClickedByUICellContainerExit()
        {
            _service.ActivateAddCellElements();
            createActive = false;
        }
        
        private void ClickedByUICellContainerExit(Port type,MenuTypes obj)
        {
            if (obj == MenuTypes.ContainerMenu)
            {
                ClickedByUICellContainerExit();
            }
        }

    }
}