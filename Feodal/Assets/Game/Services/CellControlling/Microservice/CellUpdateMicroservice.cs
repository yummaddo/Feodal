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
        public SimpleMenuExitCallBack menuExitCallBack;
        public SimpleMenuCellDeleteCallBack cellDeleteCallBack;
        public SimpleMenuCellUpdateCallBack cellCallBack;
        private CellService _service;
        private ICellContainer _selected;
        private Cell _selectedCell;
        protected override void OnAwake()
        {
            _service = SessionStateManager.Instance.Container.Resolve<CellService>();
        }

        protected override void OnStart()
        {           
            _service = SessionStateManager.Instance.Container.Resolve<CellService>();
            Proxy.Connect<CellUpdateProvider, Cell>(OnCellTryToUpdate);
            Proxy.Connect<CellSelectProvider, Cell>(OnCellSelected);
            Proxy.Connect<CellContainerElementProvider, IUICellContainerElement>(OnBuildSelected);
        }

        private void OnBuildSelected(IUICellContainerElement obj)
        {
            _selectedCell.selection.UnSelect();
            menuExitCallBack.OnClick?.Invoke(MenuTypes.BuildingMenu);
            Debug.Log(obj.State.Data.ExternalName);
            _selectedCell.MigrateToNewState(obj.State.Data);
        }

        private void OnCellSelected(Cell cell)
        {
            cell.selection.Select();
            cellDeleteCallBack.OnClick?.Invoke(cell.container.Data);
            _selectedCell = cell;
            _selected = cell.container.Data;
        }
        private void OnCellTryToUpdate(Cell cell)
        {
            cellCallBack.OnClick?.Invoke(cell.container.Data);
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