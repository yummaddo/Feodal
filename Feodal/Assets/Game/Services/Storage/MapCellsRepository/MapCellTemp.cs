using HexEngine;
using UnityEngine;

namespace Game.Services.Storage.MapCellsRepository
{
    public class MapCellTemp : Temp<MapCellEncoded, HexCoords, GameObject>
    {
        
        /// <summary>
        /// In MapCellTemp SumAmounts return the second object and swap them in data_storage
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override GameObject SumAmounts(GameObject a, GameObject b)
        {
            return b;
        }
        protected override HexCoords GetIdentifierByEncoded(MapCellEncoded encoded)
        {
            return encoded.cellCoord;
        }
    }
}