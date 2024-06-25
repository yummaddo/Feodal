using System.Collections.Generic;
using Game.Cells;
using Game.DataStructures.Abstraction;
using Game.RepositoryEngine.Abstraction;
using Game.Utility;
using HexEngine;

namespace Game.RepositoryEngine.MapCellsRepository
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
            var encode = new MapCellEncoded(cell.containerName, cell.stateName, cell.Position.Global,
                cell.Position.CellHexCoord, cell.Distance);

            if (EncodeByIdentifier.TryGetValue(encode.cellCoord, out var encodeRepository))
            {
                if (encode.Is(encodeRepository))
                {
                    Debugger.Logger($"Temp Cell in [{encodeRepository.cellCoord}] changed on [{encode.containerStateName}]");
                    Data.Remove(encodeRepository);
                    Data.Add(encode, cell.State.ExternalName);
                }
                else
                {
                    Debugger.Logger($"Temp Cell in [{encodeRepository.cellCoord}] created on [{encode.containerStateName}]");
                    Temped(encode, cell.State.ExternalName);
                }
            }
            else
            {
                Temped(encode, cell.State.ExternalName);
            }
        }
        private void CellServiceCellDestroy(Cell cell)
        {
            Data.Remove(EncodeByIdentifier[cell.Position.CellHexCoord]);
            Debugger.Logger($"Remove Cell [Destroy] in MapCellRepository [{cell.Position.CellHexCoord}:{cell.containerName}", ContextDebug.Application, Process.Update);
        }
        private void CellServiceCellChange(ICellState from, ICellState to, Cell cell)
        {
            var key = EncodeByIdentifier[cell.Position.CellHexCoord];
            cell.State = to;
            
            Debugger.Logger($"Change Remove Cell [Change] in MapCellRepository [{key.containerName}:{key.containerStateName}]", ContextDebug.Application, Process.Update);
            Data.Remove(key);
            EncodeByIdentifier.Remove(cell.Position.CellHexCoord);
            Debugger.Logger($"Change Add    Cell [Change] in MapCellRepository [{cell.containerName}:{cell.stateName}]", ContextDebug.Application, Process.Update);
            var encode = new MapCellEncoded(cell.containerName, to.ExternalName, cell.Position.Global,
                cell.Position.CellHexCoord, cell.Distance);
            Data.Add(encode, to.ExternalName);
            EncodeByIdentifier.Add(cell.Position.CellHexCoord,encode);
        }
        private void CellServiceCellAdded(Cell cell, List<ICellPosition> potentialPositions)
        {
            TempedCell(cell);
            Debugger.Logger($"Load Cell [Creation] in MapCellRepository [{cell.Position.CellHexCoord}:{cell.containerName}", ContextDebug.Application, Process.Load);
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