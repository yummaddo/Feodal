using System.Collections.Generic;
using System.Linq;
using Game.CallBacks.CallbackClick.Button;
using Game.CallBacks.CallbackClick.Simple;
using Game.DataStructures;
using Game.DataStructures.Abstraction;
using Game.RepositoryEngine.MapCellsRepository;
using Game.Services.CellServices;
using Game.Services.ProxyServices;
using Game.Services.StorageServices;
using Game.UI.Menu;
using Game.Utility;
using HexEngine;
using UnityEngine;

namespace Game.Cells
{
    public class CellMap : MonoBehaviour
    {
        public SimpleMenuTypesCloseCallBack closeCallBackMenuTypesClose;
        public List<CellContainer> containers;
        public float distance = 1.8f;
        public Cell baseCells;
        public Transform root;
        public StorageService storageService;
        
        private List<Cell> _hexCells;
        private List<HexCoords> _hexIdentifier;
        private CellService _service;
        private Dictionary<string, ICellContainer> _cellContainers;
        private Dictionary<string, ICellState> _cellStates;
        private Dictionary<HexCoords, Cell> _hexCellsIdentifier;
        
        private void InitAllData()
        {
            _hexCellsIdentifier = new Dictionary<HexCoords, Cell>();
            _cellContainers = new Dictionary<string, ICellContainer>();
            _cellStates = new Dictionary<string, ICellState>();
            _hexIdentifier = new List<HexCoords>();
            _hexCells = new List<Cell>();
            foreach (var container in containers)
            {
                _cellContainers.Add(container.containerName, container.Data);
                foreach (var state in container.states) 
                    _cellStates.Add(state.externalName, state.Data);
            }
        }
        private void Awake()
        {
            storageService.OnCellsMapRepositoryInit += StorageServiceCellsMapRepositoryInit;
        }
        public int GetCountOfCellState(ICellState state) => _hexCells.Count(t => state.ExternalName == t.State.ExternalName);
        public int GetCellCount(Seed intoDataSeed) => _hexCells.Count(t => intoDataSeed.Is(t.container.Seed));
        public int GetSeedCount(Seed intoDataSeed) => (int)storageService.GetResourceTemp().GetValueAmount(intoDataSeed.title);
        
        private void StorageServiceCellsMapRepositoryInit()
        {
            InitAllData();
            UpdateMapAddition(baseCells);
            _service.CellAdded(baseCells, FindFreeCoordsList(), false);
            
            foreach (var mapKey in  storageService.GetCellMapTemp().GetAllResourceData)
            {
                var mapCreation = mapKey.Key;
                var container = _cellContainers[mapCreation.containerName];
                Debugger.Logger($"{container.Initial.ExternalName} == {mapKey.Key} == {mapKey.Key.containerName} == {mapKey.Value}");
                if (container.Initial.ExternalName != mapKey.Value)
                    LoadContainerStateFromMapDatabase(mapKey.Key, container, _cellStates[mapKey.Value]);
                else 
                    LoadContainerFromMapDatabase(mapKey.Key, container);
            }
            _service.CellMapInitial(this);
        }
        public void InitCellMap(CellService service)
        {
            _service = service;
            baseCells.Initialization(_service,0, root.position, distance);
        }
        public void TryAddCell(Cell addable, Vector3 position, bool invocation = true)
        {
            addable.Initialization(_service,_hexCells.Count, position, distance, invocation);
            if (!_hexIdentifier.Contains(addable.Position.CellHexCoord))
            {
                UpdateMapAddition(addable);
                
                _service.CellCreated(addable, invocation);
                _service.CellAdded(addable, FindFreeCoordsList(), invocation);
                
                closeCallBackMenuTypesClose.OnCallBackInvocation?.Invoke(Porting.Type<ButtonExitMenuCallBack>(),MenuTypes.ContainerMenu);
            }
        }

        private void LoadContainerFromMapDatabase(MapCellEncoded encoded, ICellContainer container)
        {
            var position = encoded.cellPosition;
            _service.TyAddCell(position,container.CellTemplate,false);
        }
        private void LoadContainerStateFromMapDatabase(MapCellEncoded encoded, ICellContainer container, ICellState state)
        {
            var position = encoded.cellPosition;
            var component = _service.TyAddCell(position,container.CellTemplate,false);
            var last= component.MigrateToNewState(state,false);
            _service.CellChange(last,state,component,false);
        }
        private void UpdateMapAddition(Cell newCell)
        {
            _hexCellsIdentifier.Add(newCell.Position.CellHexCoord,newCell);
            _hexIdentifier.Add(newCell.Position.CellHexCoord);
            _hexCells.Add(newCell);
        }
        
        internal List<ICellPosition> FindFreeCoordsList()
        {
            List<ICellPosition> coordsList = new List<ICellPosition>();
            List<HexCoords> coordsHexList = new List<HexCoords>();
            IList<HexCoords> coordsListExisted = _hexCellsIdentifier.Keys.ToList();
            foreach (var hexCell in _hexCellsIdentifier)
            {
                foreach (var coord in hexCell.Key.Neighbours())
                {
                    if (!coordsHexList.Contains(coord) && !coordsListExisted.Contains(coord))
                    {
                        coordsHexList.Add(coord);
                        Vector3 vector = coord.Offset()*distance;
                        ICellPosition position = new CellPosition(0, vector, null, distance);
                        coordsList.Add(position);
                    }
                }
            }
            return coordsList;
        }
        internal void UpdateMapRemove(Cell newCell)
        {
            _hexCellsIdentifier.Remove(newCell.Position.CellHexCoord);
            _hexIdentifier.Remove(newCell.Position.CellHexCoord);
            _hexCells.Remove(newCell);
        }
    }
}