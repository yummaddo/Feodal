using Game.Services.Abstraction.Service;
using UnityEngine;

namespace Game.Services.Control
{
    public class ControlService : AbstractService
    {
        [SerializeField] internal Camera cameraInstance;
        [SerializeField] internal Transform cameraTarget;
        
        [SerializeField] 
        [Range(10f, 200f)]
        internal float magnitudeDivision = 80;
        
        [SerializeField] internal AnimationCurve speedElevator;
        protected override void OnAwake()
        {
            Debugger.Logger("Service Pre-Initialization (Awake): Control");
        }

        protected override void OnStart()
        {
            Debugger.Logger("Service Initialization (Start): Input");
        }

        public void MoveCamera(float speed, Vector2 fromScreenStart, Vector2 toScreenEnd, Vector2 distance)
        {
            var elevator = distance.magnitude/magnitudeDivision;
            var elevate = speedElevator.Evaluate(elevator);
            var vectorNormal = toScreenEnd - fromScreenStart;
            
            var transformed = new Vector3(-vectorNormal.y,0,  vectorNormal.x).normalized
                              *speed
                              *elevate
                              *Time.deltaTime;
            
            cameraTarget.transform.position += transformed;
        }
    }
}