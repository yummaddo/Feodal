using System;
using System.Threading.Tasks;
using Game.Services.Abstraction;
using Game.Utility;
using UnityEngine;

namespace Game.Services.ControlServices
{
    public class ControlService : AbstractService
    {
        [SerializeField] internal Camera cameraInstance;
        [SerializeField] internal Transform cameraTarget;
        protected override Task OnAwake(IProgress<float> progress)
        {
            Debugger.Logger("Service Pre-Initialization (Awake): Control");
            return Task.CompletedTask;
        }

        protected override Task OnStart(IProgress<float> progress)
        {
            Debugger.Logger("Service Initialization (Start): Input");
            return Task.CompletedTask;
        }
        public void MoveCamera(Vector3 move, float speed, Vector3 direct)
        {
            cameraTarget.transform.position += move;
        }
    }
}