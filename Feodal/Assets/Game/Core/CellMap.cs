using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Game.Core.Abstraction;
using Game.Core.Cells;
using Game.Core.DataStructures;
using Game.Services.CellControlling;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.ClickCallback.Simple;
using Game.Services.Storage;
using Game.Services.Storage.MapCellsRepository;
using Game.UI.Menu;
using HexEngine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Core
{
    public class CellMap : MonoBehaviour
    {
        public SimpleMenuExitCallBack callBackMenuExit;
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
                foreach (var state in container.states) _cellStates.Add(state.externalName, state.Data);
            }
        }

        public void InitCellMap(CellService service)
        {
            _service = service;
            storageService.OnCellsMapRepositoryInit += StorageServiceCellsMapRepositoryInit;
            baseCells.Initialization(_service,0, root.position, distance);
        }

        private void StorageServiceCellsMapRepositoryInit()
        {
            InitAllData();
            UpdateMap(baseCells);
            _service.CellAdded(baseCells, FindFreeCoordsList(), false);
            var temp = storageService.GetCellMapTemp();
            foreach (var mapKey in temp.GetAllResourceData)
            {
                var mapCreation = mapKey.Key;
                var container = _cellContainers[mapCreation.containerName];
                Debugger.Logger($"{container.Initial.ExternalName} == {mapKey.Key.containerStateName} == {mapKey.Key.containerName} == {mapKey.Key.containerStateName}");
                if (container.Initial.ExternalName != mapKey.Value)
                {
                    try
                    {
                        var state = _cellStates[mapCreation.containerName];
                        LoadContainerStateFromMapDatabase(mapKey.Key, container, state);
                    }
                    catch (Exception e)
                    {
                        Debugger.Logger($"{container.Initial.ExternalName} != {mapKey.Value}", Process.TrashHold);
                        Debugger.Logger(e.Message, Process.TrashHold);
                    }
                }
                else LoadContainerFromMapDatabase(mapKey.Key, container);
            }
        }

        private void LoadContainerFromMapDatabase(MapCellEncoded encoded, ICellContainer container)
        {
            var position = encoded.cellPosition;
            var obj = Instantiate(container.CellTemplate, position, Quaternion.identity, root);
            var component = obj.GetComponent<Cell>();
            if (component == null) throw new Exception($"{container.CellTemplate} doesnt contain Cell component");
            TryAddCell(component, position, false);
        }
        private void LoadContainerStateFromMapDatabase(MapCellEncoded encoded, ICellContainer container, ICellState state)
        {
            var position = encoded.cellPosition;
            var obj = Instantiate(container.CellTemplate, position, Quaternion.identity, root);
            var component = obj.GetComponent<Cell>();
            if (component == null) throw new Exception($"{container.CellTemplate} doesnt contain Cell component");
            TryAddCell(component, position, false);
            component.MigrateToNewState(state,false);
        }
        public int GetCountOfCellState(ICellState state) { return 0; }
        public int GetCellCount(IResource intoData) { return 0; }
        private void UpdateMap(Cell newCell)
        {
            _hexCellsIdentifier.Add(newCell.Position.CellHexCoord,newCell);
            _hexIdentifier.Add(newCell.Position.CellHexCoord);
            _hexCells.Add(newCell);
        }
        private List<ICellPosition> FindFreeCoordsList()
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
        public void TryAddCell(Cell addable, Vector3 position, bool invocation = true)
        {
            addable.Initialization(_service,_hexCells.Count, position, distance, invocation);
            
            if (!_hexIdentifier.Contains(addable.Position.CellHexCoord))
                UpdateMap(addable);
            
            _service.CellAdded(addable, FindFreeCoordsList(), invocation);
            
            callBackMenuExit.OnClick?.Invoke(MenuTypes.ContainerMenu);
        }
    }
}