using Game.Services.Abstraction.Service;
using UnityEngine;

namespace Game.Services.Control
{
    public class ControlService : AbstractService
    {
        [SerializeField] internal Camera cameraInstance;
        [SerializeField] internal Transform cameraTarget;
        
        protected override void OnAwake()
        {
            Debugger.Logger("Service Pre-Initialization (Awake): Control");
        }

        protected override void OnStart()
        {
            Debugger.Logger("Service Initialization (Start): Input");
        }

        public void MoveCamera(float speed, Vector2 fromScreenStart, Vector2 toScreenEnd)
        {
            var vectorNormal = toScreenEnd - fromScreenStart;
            var transformed = new Vector3(vectorNormal.y,0,  vectorNormal.x).normalized*speed*Time.deltaTime;
            cameraTarget.transform.position += transformed;
        }
    }
}