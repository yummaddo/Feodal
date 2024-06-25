using Game.RepositoryEngine.Abstraction;
using HexEngine;

namespace Game.RepositoryEngine.MapCellsRepository
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