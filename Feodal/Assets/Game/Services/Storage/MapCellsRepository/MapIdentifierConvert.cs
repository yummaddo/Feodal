using Game.Services.Storage.Abstraction;
using HexEngine;

namespace Game.Services.Storage.MapCellsRepository
{
    public class MapIdentifierConvert : IIdentifier<HexCoords,MapCellEncoded>
    {
        public MapIdentifierConvert() { }
        public HexCoords GetEncodedIdentifier(MapCellEncoded amount)
        {
            return amount.cellCoord;
        }
    }
    

}