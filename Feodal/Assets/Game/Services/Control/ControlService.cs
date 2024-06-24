using System;
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
        public void MoveCamera(Vector3 move, float speed, Vector3 direct)
        {
            cameraTarget.transform.position += move;
        }
    }
}