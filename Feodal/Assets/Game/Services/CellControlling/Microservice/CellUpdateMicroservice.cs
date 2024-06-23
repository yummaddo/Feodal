using System;
using Game.Core;
using Game.Core.Abstraction;
using Game.Core.Abstraction.UI;
using Game.Core.Cells;
using Game.Meta;
using Game.Services.Abstraction.MicroService;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.ClickCallback.Simple;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;
using UnityEngine;

namespace Game.Services.CellControlling.Microservice
{
    public class CellUpdateMicroservice : AbstractMicroservice<CellService>
    {
        public SimpleMenuTypesCloseCallBack menuTypesCloseCallBack;
        public SimpleCellContainerCallBack cellCallBack;
        private CellService _service;
        private ICellContainer _selected;
        private Cell _selectedCell;
        protected override void OnAwake()
        {
            _service = SessionStateManager.Instance.ServiceLocator.Resolve<CellService>();
        }

        protected override void OnStart()
        {           
            _service = SessionStateManager.Instance.ServiceLocator.Resolve<CellService>();
            Proxy.Connect<CellProvider, Cell, CellUpdatedDetector>( OnCellTryToUpdate);
            Proxy.Connect<CellProvider, Cell, CellUpdatedDetector>( OnCellSelected);
            
            Proxy.Connect<CellContainerElementProvider, IUICellContainerElement,UIMenuBuilding>( OnBuildSelected);
        }

        private void OnBuildSelected(Port type,IUICellContainerElement obj)
        {
            _selectedCell.selection.UnSelect();
            menuTypesCloseCallBack.OnCallBackInvocation?.Invoke(type,MenuTypes.BuildingMenu);
            Debugger.Logger(obj.State.Data.ExternalName, Process.Action);
            _selectedCell.MigrateToNewState(obj.State.Data);
        }

        private void OnCellSelected(Port type,Cell cell)
        {
            cell.selection.Select();
            // cellDeleteCallBack.OnClick?.Invoke(type,cell.container.Data);
            _selectedCell = cell;
            _selected = cell.container.Data;
        }
        private void OnCellTryToUpdate(Port type,Cell cell)
        {
            cellCallBack.OnCallBackInvocation?.Invoke(type,cell.container.Data);
            _selectedCell = cell;
            _selected = cell.container.Data;
        }
        protected override void ReStart()
        {
        }

        protected override void Stop()
        {
        }
    }
}