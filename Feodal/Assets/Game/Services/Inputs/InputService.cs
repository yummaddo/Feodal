using System;
using Game.Core.Cell;
using Game.Services.Abstraction.Service;
using UnityEngine;

namespace Game.Services.Inputs
{
    public sealed class InputService : AbstractService
    {
        public Camera mainCamera;
        private event Action<Camera> OnTryGetCamera;
        public event Action OnClickCloseVoid;
        public event Action OnClickPrecessedVoid;
        public event Action OnClickEnteredVoid;
        public event Action<GameObject> OnClickedByObject;
        public event Action<GameObject,CellAddDetector> OnClickedByAddCellObject;

        internal void ClickedByAddCellObject(GameObject sendObject, CellAddDetector detector) => OnClickedByAddCellObject?.Invoke(sendObject,detector);
        internal void ClickedByObject(GameObject sendObject) => OnClickedByObject?.Invoke(sendObject);
        internal void ClickCloseVoid() => OnClickCloseVoid?.Invoke();
        internal void ClickPrecessedVoid() => OnClickPrecessedVoid?.Invoke();
        internal void ClickEnteredVoid() => OnClickEnteredVoid?.Invoke();
        

        protected override void OnAwake()
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
        }

        protected override void OnStart()
        {
            Debugger.Logger("Service Initialization (Start): Input");
            if (!mainCamera)
            {
                mainCamera = Camera.main;
                OnTryGetCamera?.Invoke(mainCamera);
            }
        }

    }
}