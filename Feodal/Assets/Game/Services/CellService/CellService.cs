using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Game.Core;
using Game.Core.Abstraction;
using Game.Core.Abstraction.UI;
using Game.Core.Cells;
using Game.Services.Abstraction.Service;
using HexEngine;
using UnityEngine;

namespace Game.Services.CellControlling
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
        protected override void OnAwake()
        {
            if (cellMap) cellMap.InitCellMap(this);
        }
        protected override void OnStart() { }
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
            if (invocation)
            {
                Debugger.Logger($"[CellService]CellDestroy:{cell.name}  PoolID:{cell.poolId} on position:{cell.Position.CellHexCoord.ToString()}", Process.Destroy);
                OnCellDestroy?.Invoke(cell);
            }
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
        
        internal void TyAddCell(CellAddDetector cellAddDetector ,IUICellContainer container)
        {
            var obj = Instantiate(container.CellTemplate, cellAddDetector.transform.position, Quaternion.identity,
                cellMap.root);
            var component = obj.GetComponent<Cell>();
            if (component == null) throw new Exception($"{container.CellTemplate} doesnt contain Cell component");
            cellMap.TryAddCell(component,cellAddDetector.transform.position, true);
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