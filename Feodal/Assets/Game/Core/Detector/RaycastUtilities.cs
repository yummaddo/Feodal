using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core.Detector
{
    public static class RaycastUtilities
    {
        public static bool PointerIsOverUI(Vector2 screenPos, GameObject GO)
        {
            var hitObject = UIRaycast(ScreenPosToPointerData(screenPos));
            return hitObject != null && hitObject.layer == LayerMask.NameToLayer("UI") && hitObject == GO;
        }
 
        public static GameObject UIRaycast(PointerEventData pointerData)
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            Debug.Log(results[0].gameObject.name + "Was raycast hit...");
            return results.Count < 1 ? null : results[0].gameObject;
        }
 
        static PointerEventData ScreenPosToPointerData(Vector2 screenPos)
            => new(EventSystem.current) { position = screenPos };
    }
}