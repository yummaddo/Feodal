using UnityEngine;

namespace Game.Core.Detector.Base
{
    public interface IColliderDetector<T> where T : Collider
    {
        public float SaveZone { get; set; }
        public T ColliderDetection { get; set; }
        public LayerMask DetectedLayer { get; set; }
        public  bool GetDetectionByPosition(Vector3 point);
    }
}