using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Cells;
using Game.DataStructures.Abstraction;
using Game.Services.Abstraction;
using Game.UI.Abstraction;
using Game.Utility;
using HexEngine;
using UnityEngine;

namespace Game.Services.CellServices
{
    public sealed class CellService : AbstractService
    {
        public GameObject primitive;
        public Transform rootForFree;
        public CellMap cellMap;
        public List<ICellPosition> FreeCellPosition;
        public List<HexCoords> FreeCellCoords;
        private Dictionary<HexCoords, GameObject> _freeCellTransform = new Dictionary<HexCoords, GameObject>();
        #region Coordinators

        public event Action<Cell, List<ICellPosition>> OnCellAdded;
        public event Action<ICellState, ICellState, Cell> OnCellChange;
        public event Action<CellMap> OnCellMapInitial;
        public event Action<Cell> OnCellCreate;
        public event Action<Cell> OnCellDestroy;
        public bool IsMapCellInitial { get; set; } = false;

        #endregion
        protected override Task OnAwake(IProgress<float> progress)
        {
            if (cellMap) cellMap.InitCellMap(this);
            return Task.CompletedTask;
        }

        protected override Task OnStart(IProgress<float> progress)
        {
            return Task.CompletedTask;
        }
        internal void PauseTheAddCellElements() { }
        internal void ActivateAddCellElements() { }
        /// <summary>
        /// CellChange Invocator
        /// </summary>
        internal void CellChange(ICellState from, ICellState to, Cell cell, bool invocation = true)
        {
            if (invocation)
            {
                Debugger.Logger($"[CellService]CellChange:{cell.name} PoolID:{cell.poolId} position:{cell.Position.CellHexCoord.ToString()}", Process.Update);
                OnCellChange?.Invoke(from,to, cell);
            }
        }
        /// <summary>
        /// CellCreated Invocator
        /// </summary>
        internal void CellCreated(Cell cell, bool invocation = true)
        {
            if (invocation)
            {
                Debugger.Logger($"[CellService]CellCreated:{cell.name} PoolID:{cell.poolId} position:{cell.Position.CellHexCoord.ToString()}", Process.Create);
                OnCellCreate?.Invoke(cell);
            }
        }
        /// <summary>
        /// CellDestroy Invocator
        /// </summary>
        internal void CellDestroy(Cell cell, bool invocation = true)
        {
            cellMap.UpdateMapRemove(cell);
            if (invocation)
            {
                Debugger.Logger($"[CellService]CellDestroy:{cell.name}  PoolID:{cell.poolId} on position:{cell.Position.CellHexCoord.ToString()}", Process.Destroy);
                OnCellDestroy?.Invoke(cell);
                cell.DestroyCell();
            }
            ClearLastFreeCellTransform( cellMap.FindFreeCoordsList() );
            FindNewFreeCellTransform();
        }
        /// <summary>
        /// CellAdded Invocator
        /// </summary>
        internal void CellAdded(Cell cell, List<ICellPosition> arg2, bool invocation = true)
        {
            ClearLastFreeCellTransform(arg2);
            FindNewFreeCellTransform();
            if (invocation)
            {
                Debugger.Logger($"[CellService]CellAdded:{cell.name}  PoolID:{cell.poolId} on position:{cell.Position.CellHexCoord.ToString()}", Process.Create);
                OnCellAdded?.Invoke(cell, arg2);
            }
        }
        
        internal Cell TyAddCell(CellAddDetector cellAddDetector ,IUICellContainer container, bool invocation = true)
        {
            var obj = Instantiate(container.CellTemplate, cellAddDetector.transform.position, Quaternion.identity,
                cellMap.root);
            var component = obj.GetComponent<Cell>();
            if (component == null) throw new Exception($"{container.CellTemplate} doesnt contain Cell component");
            cellMap.TryAddCell(component,cellAddDetector.transform.position, invocation);
            return component;
        }
        internal Cell TyAddCell(Vector3 pos ,IUICellContainer container, bool invocation = true)
        {
            var obj = Instantiate(container.CellTemplate, pos, Quaternion.identity,
                cellMap.root);
            var component = obj.GetComponent<Cell>();
            if (component == null) throw new Exception($"{container.CellTemplate} doesnt contain Cell component");
            cellMap.TryAddCell(component,pos, invocation);
            return component;
        }
        internal Cell TyAddCell(Vector3 pos ,GameObject containerTemplate, bool invocation = true)
        {
            var obj = Instantiate(containerTemplate, pos, Quaternion.identity,
                cellMap.root);
            var component = obj.GetComponent<Cell>();
            if (component == null) throw new Exception($"{containerTemplate} doesnt contain Cell component");
            cellMap.TryAddCell(component,pos, invocation);
            return component;
        }
        private void ClearLastFreeCellTransform(List<ICellPosition> arg2)
        {
            FreeCellPosition = arg2;
            FreeCellCoords = new List<HexCoords>();
            foreach (var obj in _freeCellTransform) Destroy(obj.Value);
        }
        private void FindNewFreeCellTransform()
        {
            _freeCellTransform = new Dictionary<HexCoords, GameObject>();
            foreach (var position in FreeCellPosition)
            {
                FreeCellCoords.Add(position.CellHexCoord);

                if (_freeCellTransform.ContainsKey(position.CellHexCoord)) continue;
                var obj = Instantiate(primitive, position.Global, Quaternion.identity, rootForFree);
                _freeCellTransform.Add(position.CellHexCoord, obj);
            }
        }
        
        internal void CellMapInitial(CellMap obj)
        {
            IsMapCellInitial = true;
            OnCellMapInitial?.Invoke(obj);
            Debugger.Logger("OnCellMapInitial", Process.Action);
        }
    }
}