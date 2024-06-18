using Game.Core.Detector.Base;
using UnityEngine;

namespace Game.Core.Detector
{
    public class SphereBorderDetector : ColliderDetector<SphereCollider>
    {
        public bool GetDetectStatus()
        {
            return indexing.CollisionIndexation.Count > 0;
        }
        public override bool GetDetectionByPosition(Vector3 point)
        {
            return DetectionProcedureDetectionByPosition(point) == expected;
        }

        private bool DetectionProcedureDetectionByPosition(Vector3 point)
        {
            LayerMask layerMaskToCheck = DetectedLayer;
            var size = Physics.OverlapSphereNonAlloc(point, SaveZone, Results, layerMaskToCheck);
            if (size > 0)
            {
                return true;
            }
            return false;
        }
    }
}