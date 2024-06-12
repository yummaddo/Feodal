using UnityEngine;

namespace Game.Core.Abstraction
{
    public interface ICellPosition
    {
        public int Identifier { get; set; }
        public Vector3 Global { get; set; }
        public Transform Root { get; set; }
    }
}