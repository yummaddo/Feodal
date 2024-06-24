using System;
using Game.Core;
using Game.Core.Abstraction;
using Game.Core.Abstraction.UI;
using Game.Core.Cells;
using Game.Core.DataStructures;
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
        private CellService _service;
        private ICellContainer _selected;
        private Cell _selectedCell;

        protected override void ReStart() { }
        protected override void Stop() { }
        protected override void OnAwake()
        {
            _service = SessionStateManager.Instance.ServiceLocator.Resolve<CellService>();
        }
        protected override void OnStart()
        {           
            _service = SessionStateManager.Instance.ServiceLocator.Resolve<CellService>();
            
            // Proxy.Connect<CellProvider, Cell, CellUpdatedDetector>( OnCellTryToUpdate);
            Proxy.Connect<CellProvider, Cell, CellUpdatedDetector>( OnCellSelected);
            Proxy.Connect<CellStateProvider, CellState,CellState>( OnBuildSelectedToCreate);
            
        }
        private void OnBuildSelectedToCreate(Port type,CellState obj)
        {
            Debugger.Logger(obj.Data.ExternalName, Process.Action);
            _selectedCell.selection.UnSelect();
            var last = _selectedCell.MigrateToNewState(obj.Data);
            _service.CellChange(last,_selectedCell.State,_selectedCell,true);
            menuTypesCloseCallBack.OnCallBackInvocation?.Invoke(type,MenuTypes.BuildingMenu);
            menuTypesCloseCallBack.OnCallBackInvocation?.Invoke(type,MenuTypes.TradeMenu);
        }
        private void OnCellSelected(Port type,Cell cell)
        {
            Debugger.Logger($"Select cell [{cell.Position.CellHexCoord}]", Process.Info);
            cell.selection.Select();
            _selectedCell = cell;
            _selected = cell.container.Data;
        }
    }
}