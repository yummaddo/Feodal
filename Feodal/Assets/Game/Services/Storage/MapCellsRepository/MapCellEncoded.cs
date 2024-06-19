using HexEngine;
using UnityEngine;

namespace Game.Services.Storage.MapCellsRepository
{
    [System.Serializable]
    public class MapCellEncoded
    {
        public string containerName;
        public string containerStateName;
        public HexCoords cellCoord;
        public Vector3 cellPosition;
        public float cellScale;
        public MapCellEncoded(string name, string state, Vector3 position, HexCoords coords, float scale)
        {
            containerName = name;
            containerStateName = state;
            cellPosition = position;
            cellScale = scale;
        }
    }
}