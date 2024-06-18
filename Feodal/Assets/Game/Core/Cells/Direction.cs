using UnityEngine;

namespace Game.Core.Cells
{
    public enum Direction
    {
        Direct0,Direct60,Direct120,
        Direct180,Direct240,Direct300
    }

    public static class DirectionExtended
    {
        public static Vector3 GetDirectionVector(Direction direction)
        {
            switch (direction)
            {
                case Direction.Direct0: return new Vector3(1, 0, 0);
                case Direction.Direct60: return new Vector3(0.5f, 0, Mathf.Sqrt(3) / 2);
                case Direction.Direct120: return new Vector3(-0.5f, 0, Mathf.Sqrt(3) / 2);
                case Direction.Direct180: return new Vector3(-1, 0, 0);
                case Direction.Direct240: return new Vector3(-0.5f, 0, -Mathf.Sqrt(3) / 2);
                case Direction.Direct300: return new Vector3(0.5f, 0, -Mathf.Sqrt(3) / 2);
                default: return Vector3.zero;
            }
        }
        public static Direction ReverseDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Direct0: return Direction.Direct180;
                case Direction.Direct60: return Direction.Direct240;
                case Direction.Direct120: return Direction.Direct300;
                case Direction.Direct180: return Direction.Direct0;
                case Direction.Direct240: return Direction.Direct60;
                case Direction.Direct300: return Direction.Direct120;
                default: return Direction.Direct0;
            }
        }
        public static Direction GetDirectionFromVector(Vector3 directionVector)
        {
            if (directionVector == new Vector3(1, 0, 0))
                return Direction.Direct0;
            else if (directionVector == new Vector3(0.5f, 0, Mathf.Sqrt(3) / 2))
                return Direction.Direct60;
            else if (directionVector == new Vector3(-0.5f, 0, Mathf.Sqrt(3) / 2))
                return Direction.Direct120;
            else if (directionVector == new Vector3(-1, 0, 0))
                return Direction.Direct180;
            else if (directionVector == new Vector3(-0.5f, 0, -Mathf.Sqrt(3) / 2))
                return Direction.Direct240;
            else if (directionVector == new Vector3(0.5f, 0, -Mathf.Sqrt(3) / 2))
                return Direction.Direct300;
            else
            {
                Debug.LogError($"Unsupported direction vector: {directionVector}");
                return Direction.Direct0; 
            }
        }
    }
}