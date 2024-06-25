using Game.Typing;
using UnityEngine;

namespace Game.Cells
{
    [System.Serializable]
    public class CellLink
    {
        public DirectionType direct;
        public CellAddDetector detector;
        public Transform cellTarget;
        public Cell cell;
    }
}