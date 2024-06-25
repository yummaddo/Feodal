using System;
using Game.DataStructures.Abstraction;
using Game.Typing;
using HexEngine;
using UnityEngine;

namespace Game.Cells
{
    public class CellPosition : ICellPosition
    {
        private static readonly Vector3[] DirectionVectors = new Vector3[]
        {
            new Vector3(1, 0, 0), // Direct0 -
            new Vector3(0.5f, 0, Mathf.Sqrt(3) / 2), // Direct60 -
            new Vector3(-0.5f, 0, Mathf.Sqrt(3) / 2), // Direct120 - 
            new Vector3(-1, 0, 0), // Direct180 -
            new Vector3(-0.5f, 0, -Mathf.Sqrt(3) / 2), // Direct240 - 
            new Vector3(0.5f, 0, -Mathf.Sqrt(3) / 2) // Direct300 - 
        };
        
        public int Identifier { get; set; }
        public Vector3 Global { get; set; }
        public HexPosition CellHexPosition { get; set; }
        public HexCoords CellHexCoord { get; set; }
        public Transform Root { get; set; }
        
        public CellPosition(int identifier, Vector3 @global, Transform root,float distance)
        {
            this.CellHexPosition = HexMath.PointToHexPosition(global, distance);
            this.CellHexCoord = HexMath.RoundPositionToCoords(this.CellHexPosition);
            Identifier = identifier;
            Global = global;
            Root = root;
        }

        public CellPosition MoveTo(DirectionType directionType, float distance)
        {
            Vector3 directionVector = DirectionVectors[(int)directionType];
            Vector3 newPosition = Global + directionVector * distance;
            return new CellPosition(Identifier, newPosition, Root,distance);
        }
        public bool Equals(Vector3 other) { return Global.Equals(other); }
        public bool Equals(int other) { return Identifier.Equals(other); }
        public bool Equals(Transform other) { return Equals(Root, other); }
        public bool Equals(ICellPosition other)
        {
            if (other == null) return false;
            return Identifier == other.Identifier &&
                   Global.Equals(other.Global) &&
                   Root == other.Root;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CellPosition)obj);
        }
        public override int GetHashCode() { return HashCode.Combine(Identifier, Global, Root); }
        public override string ToString() { return $"CellPosition: [Identifier={Identifier}, Global={Global}, Root={Root?.name}]"; }
    }
}