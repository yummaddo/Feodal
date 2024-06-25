using UnityEngine;

namespace Game.Typing
{
    public static class DirectionExtended
    {
        public static Vector3 GetDirectionVector(DirectionType directionType)
        {
            switch (directionType)
            {
                case DirectionType.Direct0: return new Vector3(1, 0, 0);
                case DirectionType.Direct60: return new Vector3(0.5f, 0, Mathf.Sqrt(3) / 2);
                case DirectionType.Direct120: return new Vector3(-0.5f, 0, Mathf.Sqrt(3) / 2);
                case DirectionType.Direct180: return new Vector3(-1, 0, 0);
                case DirectionType.Direct240: return new Vector3(-0.5f, 0, -Mathf.Sqrt(3) / 2);
                case DirectionType.Direct300: return new Vector3(0.5f, 0, -Mathf.Sqrt(3) / 2);
                default: return Vector3.zero;
            }
        }
        public static DirectionType ReverseDirection(DirectionType directionType)
        {
            switch (directionType)
            {
                case DirectionType.Direct0: return DirectionType.Direct180;
                case DirectionType.Direct60: return DirectionType.Direct240;
                case DirectionType.Direct120: return DirectionType.Direct300;
                case DirectionType.Direct180: return DirectionType.Direct0;
                case DirectionType.Direct240: return DirectionType.Direct60;
                case DirectionType.Direct300: return DirectionType.Direct120;
                default: return DirectionType.Direct0;
            }
        }
        public static DirectionType GetDirectionFromVector(Vector3 directionVector)
        {
            if (directionVector == new Vector3(1, 0, 0))
                return DirectionType.Direct0;
            else if (directionVector == new Vector3(0.5f, 0, Mathf.Sqrt(3) / 2))
                return DirectionType.Direct60;
            else if (directionVector == new Vector3(-0.5f, 0, Mathf.Sqrt(3) / 2))
                return DirectionType.Direct120;
            else if (directionVector == new Vector3(-1, 0, 0))
                return DirectionType.Direct180;
            else if (directionVector == new Vector3(-0.5f, 0, -Mathf.Sqrt(3) / 2))
                return DirectionType.Direct240;
            else if (directionVector == new Vector3(0.5f, 0, -Mathf.Sqrt(3) / 2))
                return DirectionType.Direct300;
            else
            {
                Debug.LogError($"Unsupported direction vector: {directionVector}");
                return DirectionType.Direct0; 
            }
        }
    }
}