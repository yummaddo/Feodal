using System;
using Game.Core.Abstraction.UI;
using Game.Core.Cells;
using Game.Meta;
using Game.Services.Abstraction.MicroService;
using Game.Services.Proxies;
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
            _service = SessionStateManager.Instance.Container.Resolve<CellService>();
        }
        private void ClickedByAddCellObject(CellAddDetector detector)
        {
            cellAddDetector = detector;
            createActive = true;
            _service.PauseTheAddCellElements();
        }
        private void ClickedByUICellContainerObject(IUICellContainer container)
        {
            _service.TyAddCell(cellAddDetector, container);
        }
        private void ClickedByUICellContainerExit()
        {
            _service.ActivateAddCellElements();
            createActive = false;
        }
        protected override void OnStart()
        {
            Proxy.Connect<CellAddProvider, CellAddDetector>(ClickedByAddCellObject);
            Proxy.Connect<CellContainerProvider, IUICellContainer>(ClickedByUICellContainerObject);
            Proxy.Connect<MenuExitProvider, MenuTypes>(ClickedByUICellContainerExit);
        }
        private void ClickedByUICellContainerExit(MenuTypes obj)
        {
            if (obj == MenuTypes.ContainerMenu)
            {
                ClickedByUICellContainerExit();
            }
        }
        protected override void ReStart() { }
        protected override void Stop() { }
    }
}