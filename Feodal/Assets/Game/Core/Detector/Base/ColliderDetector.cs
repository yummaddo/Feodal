using UnityEngine;

namespace Game.Core.Detector.Base
{
    public abstract class ColliderDetector<TCollider> : MonoBehaviour, IColliderDetector<TCollider> where TCollider : Collider
    {
        public bool expected = false;
        public DetectorIndexing indexing;
        internal bool Status = false;

        protected readonly Collider[] Results = new Collider[1];

        [field:SerializeField]public float SaveZone { get; set; }
        [field:SerializeField]public TCollider ColliderDetection { get; set; }
        [field:SerializeField]public LayerMask DetectedLayer { get; set; }
       protected string GetDetectedLayerName()
       {
           int layerNumber = (int)Mathf.Log(DetectedLayer.value, 2);
           return  LayerMask.LayerToName((int)layerNumber);
       }
       protected string GetTriggeredLayerName(int layer)
       {
           return LayerMask.LayerToName(layer);
       }
       public abstract bool GetDetectionByPosition(Vector3 point);
    }
    
}