using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Core.Abstraction;
using Game.Core.Abstraction.UI;
using Game.Core.Cells;
using Game.Services.Abstraction.Service;
using Game.Services.Proxies;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;
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
        public event Action<Cell, List<ICellPosition>> OnCellAdded;
        protected override void OnAwake()
        {
        }
        protected override void OnStart()
        {
            if (cellMap)
            {
                cellMap.InitCellMap(this);
            }
        }
        internal void PauseTheAddCellElements() { }
        internal void ActivateAddCellElements() { }

        internal void TyAddCell(CellAddDetector cellAddDetector ,IUICellContainer container)
        {
            var obj = Instantiate(container.CellTemplate, cellAddDetector.transform.position, Quaternion.identity,
                cellMap.root);
            var component = obj.GetComponent<Cell>();
            if (component == null) throw new Exception($"{container.CellTemplate} doesnt contain Cell component");
            cellMap.TryAddCell(component,cellAddDetector.transform.position);
            
        }
        internal void CellAdded(Cell arg1, List<ICellPosition> arg2)
        {
            FreeCellPosition = arg2;
            FreeCellCoords = new List<HexCoords>();
            foreach (var obj in _freeCellTransform)
            {
                Destroy(obj.Value);
            }
            _freeCellTransform = new Dictionary<HexCoords, GameObject>();
            foreach (var position in FreeCellPosition)
            {
                FreeCellCoords.Add(position.CellHexCoord);
                
                if (_freeCellTransform.ContainsKey(position.CellHexCoord)) 
                    continue;
                var obj = Instantiate(primitive,position.Global ,Quaternion.identity,rootForFree);
                _freeCellTransform.Add(position.CellHexCoord, obj);
                
            }
            Debugger.Logger($"Add Cell:{arg1.name} into map with PoolID:{arg1.poolId} on position:{arg1.Position.Global.ToString()}");
            OnCellAdded?.Invoke(arg1, arg2);
        }
    }
}