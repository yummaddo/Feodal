using System;
using Game.Core.Abstraction.UI;
using Game.Core.Cells;
using Game.Meta;
using Game.Services.Abstraction.MicroService;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;
using UnityEngine;

namespace Game.Services.CellControlling.Microservice
{
    public class CellCreateMicroservice : AbstractMicroservice<CellService>
    {
        private CellService _service;
        public bool createActive = false;
        public CellAddDetector cellAddDetector  = null;
        public GameObject template;
        protected override void OnAwake()
        {
            _service = SessionStateManager.Instance.ServiceLocator.Resolve<CellService>();
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
        protected override void OnStart()
        {
            Proxy.Connect<CellAddDetectorProvider, CellAddDetector,CellAddDetector>( ClickedByAddCellObject);
            Proxy.Connect<UICellContainerProvider, IUICellContainer,UIMenuContainer>( ClickedByUICellContainerObject);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>( ClickedByUICellContainerExit);
        }
        protected override void ReStart() { }
        protected override void Stop() { }
    }
}