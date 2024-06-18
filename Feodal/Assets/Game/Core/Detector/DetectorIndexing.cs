using System.Collections.Generic;
using Game.Core.Detector.Base;
using Game.Meta;
using Game.Services.Inputs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core.Detector
{
    public class DetectorIndexing : MonoBehaviour
    {
        
        internal List<int> CollisionIndexation;
        public List<ColliderDetector<SphereCollider>> detectorsSphere;
        public List<ColliderDetector<CapsuleCollider>> detectorsCapsule;
        private InputService _service;
        private void Awake()
        {
            _service = SessionStateManager.Instance.Container.Resolve<InputService>();
        }
        public bool CheckRaycastHit(string tagForCast, DetectorIndexing indexing)
        {
            Ray ray = _service.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
                if (hit.collider.CompareTag(tagForCast))
                {
                    var obj = hit.collider.GetComponentInParent<DetectorIndexing>();
                    if (obj.GetInstanceID() == indexing.GetInstanceID())
                    {
                        return true;
                    }
                }
            return false;
        }
        public bool CheckRaycastHit(string tagForCast)
        {
            Ray ray = _service.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
                if (hit.collider.CompareTag(tagForCast))
                {
                    return true;
                }
            return false;
        }
        public bool DetectPoint(Vector3 point)
        {
            foreach (var detector in detectorsSphere)
            {
                if (detector.GetDetectionByPosition(point)) return true;
            }
            foreach (var detector in detectorsCapsule)
            {
                if (detector.GetDetectionByPosition(point)) return true;
            }
            return false;
        }
        

    }
}