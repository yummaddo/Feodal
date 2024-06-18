using UnityEngine;

namespace Game.Core.Cells
{
    [System.Serializable]
    public class CellLink
    {
        public Direction direct;
        public CellAddDetector detector;
        public Transform cellTarget;
        public Cell cell;
    }
}