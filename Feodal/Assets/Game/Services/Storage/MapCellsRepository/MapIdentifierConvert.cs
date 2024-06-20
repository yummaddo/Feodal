using Game.Services.Storage.MapCellsRepository;
using HexEngine;

namespace Game.Services.Storage.Abstraction
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