using Game.Services.Storage.MapCellsRepository;
using HexEngine;

namespace Game.Services.Storage.Abstraction
{
    public class CellIdentifierConvert : IIdentifier<HexCoords,MapCellEncoded>
    {
        public CellIdentifierConvert() { }
        public HexCoords GetEncodedIdentifier(MapCellEncoded amount)
        {
            return amount.cellCoord;
        }
    }
    

}