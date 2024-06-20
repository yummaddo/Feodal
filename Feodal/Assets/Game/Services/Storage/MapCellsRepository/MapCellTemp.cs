using System.Collections.Generic;
using System.Security.Cryptography;
using Game.Core;
using Game.Core.Abstraction;
using Game.Services.CellControlling;
using Game.Services.Storage.Abstraction;
using HexEngine;
using UnityEngine;

namespace Game.Services.Storage.MapCellsRepository
{
    public class MapCellTemp : Temp<MapCellEncoded, HexCoords, string>
    {
        internal override void Injection()
        {
            base.Injection();
            CellService.OnCellAdded += CellServiceCellAdded;
            CellService.OnCellChange += CellServiceCellChange;
            CellService.OnCellDestroy += CellServiceCellDestroy;
        }

        private void TempedCell(Cell cell)
        {
            var encode = new MapCellEncoded(cell.container.containerName, cell.State.ExternalName, cell.Position.Global,
                cell.Position.CellHexCoord, cell.Distance);
            Temped(encode, cell.State.ExternalName);
        }
        private void CellServiceCellDestroy(Cell cell)
        {
            TempedCell(cell);
            Debugger.Logger($"Load Cell [Destroy] in MapCellRepository [{cell.container.containerName}", ContextDebug.Application, Process.Load);
        }
        private void CellServiceCellChange(ICellState from, ICellState to, Cell cell)
        {
            TempedCell(cell);
            Debugger.Logger($"Load Cell [Change] in MapCellRepository [{cell.container.containerName}", ContextDebug.Application, Process.Load);
        }
        private void CellServiceCellAdded(Cell cell, List<ICellPosition> potentialPositions)
        {
            TempedCell(cell);
            Debugger.Logger($"Load Cell [Creation] in MapCellRepository [{cell.container.containerName}", ContextDebug.Application, Process.Load);
        }
        /// <summary>
        /// In MapCellTemp SumAmounts return the second object and swap them in data_storage
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override string SummedAmounts(string a, string b)
        {
            return b;
        }

        protected override HexCoords GetIdentifierByEncoded(MapCellEncoded encoded)
        {
           return encoded.cellCoord;
        }
        internal MapCellTemp(IIdentifier<HexCoords, MapCellEncoded> identifier) : base(identifier)
        {
        }
    }
}