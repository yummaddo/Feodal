using System;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Simple;
using Game.Cells;
using Game.DataStructures;
using Game.DataStructures.Abstraction;
using Game.Services.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu;
using Game.Utility;

namespace Game.Services.CellServices.Microservice
{
    public class CellUpdateMicroservice : AbstractMicroservice<CellService>
    {
        public SimpleMenuTypesCloseCallBack menuTypesCloseCallBack;
        private ICellContainer _selected;
        private CellService _service;
        private Cell _selectedCell;
        protected override Task OnAwake(IProgress<float> progress)
        {
            _service = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellService>();
            progress.Report(1f);
            return Task.CompletedTask;
        }
        protected override Task OnStart(IProgress<float> progress)
        {           
            _service = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<CellService>();
            // Proxy.Connect<CellProvider, Cell, CellUpdatedDetector>( OnCellTryToUpdate);
            Proxy.Connect<CellProvider, Cell, CellUpdatedDetector>( OnCellSelected );
            Proxy.Connect<CellProvider, Cell>( OnCellDeleteMenu,  Port.SelectionCellBase);
            Proxy.Connect<CellProvider, Cell>( OnCellSelectedDeleteContainer, Port.CellDeleteContainer);
            Proxy.Connect<CellProvider, Cell>( OnCellSelectedDeleteBuild, Port.CellDeleteBuilding);
            Proxy.Connect<CellStateProvider, CellState,CellState>( OnBuildSelectedToCreate);
            return Task.CompletedTask;
        }
        
        private void OnCellSelectedDeleteBuild(Port port, Cell cell)
        {
            if (cell.IsBaseState)
            {
                Debugger.Logger($"{cell.State.ExternalName} IsBaseState can not build delete processing", Process.Info);
                return;
            }
            _selectedCell.selection.UnSelect();
            var last = cell.MigrateToBase();
            
            _service.CellChange(last,cell.State,cell,true);
            
            menuTypesCloseCallBack.OnCallBackInvocation?.Invoke(port,MenuTypes.BuildingMenu);
            menuTypesCloseCallBack.OnCallBackInvocation?.Invoke(port,MenuTypes.TradeMenu);
            menuTypesCloseCallBack.OnCallBackInvocation?.Invoke(port,MenuTypes.CellSetterMenu);
        }
        private void OnCellSelectedDeleteContainer(Port port, Cell cell)
        {
            _service.CellDestroy(cell);
            menuTypesCloseCallBack.OnCallBackInvocation?.Invoke(port,MenuTypes.BuildingMenu);
            menuTypesCloseCallBack.OnCallBackInvocation?.Invoke(port,MenuTypes.TradeMenu);
            menuTypesCloseCallBack.OnCallBackInvocation?.Invoke(port,MenuTypes.CellSetterMenu);
        }
        
        private void OnCellDeleteMenu(Port port, Cell cell) => OnCellSelected(port, cell);
        private void OnBuildSelectedToCreate(Port port,CellState cell)
        {
            Debugger.Logger(cell.Data.ExternalName, Process.Action);
            _selectedCell.selection.UnSelect();
            var last = _selectedCell.MigrateToNewState(cell.Data);
            _service.CellChange(last,_selectedCell.State,_selectedCell,true);
            menuTypesCloseCallBack.OnCallBackInvocation?.Invoke(port,MenuTypes.BuildingMenu);
            menuTypesCloseCallBack.OnCallBackInvocation?.Invoke(port,MenuTypes.TradeMenu);
        }
        private void OnCellSelected(Port port,Cell cell)
        {
            Debugger.Logger($"Select cell [{cell.Position.CellHexCoord}]", Process.Info);
            cell.selection.Select();
            _selectedCell = cell;
            _selected = cell.container.Data;
        }
    }
}