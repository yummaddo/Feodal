using HexEngine;
using UnityEngine;

namespace Game.RepositoryEngine.MapCellsRepository
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
            cellCoord = coords;
        }

        public bool Is(MapCellEncoded encoded)
        {
            return cellCoord == encoded.cellCoord;
        }
    }
}