using UnityEngine;

namespace Game.Core.Cell
{
    [System.Serializable]
    public class CellLink
    {
        public Direction direct;
        public Transform cellTarget;
        public Cell cell;
        
    }
}