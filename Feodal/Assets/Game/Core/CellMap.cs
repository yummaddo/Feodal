using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Abstraction;
using Game.Core.Cells;
using Game.Services.CellControlling;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.ClickCallback.Simple;
using Game.UI.Menu;
using HexEngine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Core
{
    public class CellMap : MonoBehaviour
    {
        internal CellService Service;
        [FormerlySerializedAs("callBackMenuExit")] public SimpleMenuExitCallBack callBackMenuExit;
        public float distance = 1.8f;
        public Cell baseCells;
        public Transform root;
        public List<Cell> hexCells;
        public Dictionary<HexCoords, Cell> HexCellsIdentifier;
        public List<HexCoords> hexIdentifier;
        public void InitCellMap(CellService service)
        {
            Service = service;
            HexCellsIdentifier = new Dictionary<HexCoords, Cell>();
            baseCells.Initialization(0, root.position, distance);
            UpdateMap(baseCells);
            
            Service.CellAdded(baseCells, FindFreeCoordsList());
        }
        public int GetCountOfCellState(ICellState state)
        {
            return 0;
        }
        public int GetCellCount(IResource intoData)
        {
            return 0;
        }
        private void UpdateMap(Cell newCell)
        {
            HexCellsIdentifier.Add(newCell.Position.CellHexCoord,newCell);
            hexIdentifier.Add(newCell.Position.CellHexCoord);
            hexCells.Add(newCell);
        }
        public List<ICellPosition> FindFreeCoordsList()
        {
            List<ICellPosition> coordsList = new List<ICellPosition>();
            List<HexCoords> coordsHexList = new List<HexCoords>();
            IList<HexCoords> coordsListExisted = HexCellsIdentifier.Keys.ToList();
            foreach (var hexCell in HexCellsIdentifier)
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
        public void TryAddCell(Cell addable, Vector3 position)
        {
            addable.Initialization(hexCells.Count, position, distance);
            if (!hexIdentifier.Contains(addable.Position.CellHexCoord))
                UpdateMap(addable);
            Service.CellAdded(addable, FindFreeCoordsList());
            callBackMenuExit.OnClick?.Invoke(MenuTypes.ContainerMenu);
        }
    }
}