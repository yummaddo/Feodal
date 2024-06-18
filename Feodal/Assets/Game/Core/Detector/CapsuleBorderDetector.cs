using Game.Core.Detector.Base;
using UnityEngine;

namespace Game.Core.Detector
{
    public class CapsuleBorderDetector : ColliderDetector<CapsuleCollider>
    {
        public override bool GetDetectionByPosition(Vector3 point)
        {
            return DetectionProcedureDetectionByPosition(point) == expected;
        }

        private bool DetectionProcedureDetectionByPosition(Vector3 point)
        {
            float radius = SaveZone;
            float height = ColliderDetection.height;
            Vector3 point1 = point + Vector3.up * height / 2f;
            Vector3 point2 = point - Vector3.up * height / 2f;
            LayerMask layerMaskToCheck = DetectedLayer;
            var size = Physics.OverlapCapsuleNonAlloc(point1,point2, radius, Results, layerMaskToCheck);
            if (size > 0)
            {
                return true;
            }
            return false;
        }
    }
}