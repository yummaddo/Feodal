using System;
using System.Threading.Tasks;
using Game.Cells;
using Game.Services.Abstraction;
using Game.Utility;
using UnityEngine;

namespace Game.Services.InputServices
{
    public sealed class InputService : AbstractService
    {
        public Camera mainCamera;
        private event Action<Camera> OnTryGetCamera;

        protected override Task OnAwake(IProgress<float> progress)
        {
            Debugger.Logger("Service Pre-Initialization (Awake): Input");
            if (!mainCamera)
            {
                mainCamera = Camera.main;
                OnTryGetCamera?.Invoke(mainCamera);
            }
            if (!mainCamera)
            {
                mainCamera = Camera.current;
                OnTryGetCamera?.Invoke(mainCamera);
            }
            return Task.CompletedTask;
        }

        protected override Task OnStart(IProgress<float> progress)
        {
            Debugger.Logger("Service Initialization (Start): Input");
            if (!mainCamera)
            {
                mainCamera = Camera.main;
                OnTryGetCamera?.Invoke(mainCamera);
            }
            return Task.CompletedTask;
        }

    }
}