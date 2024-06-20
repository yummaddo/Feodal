using System;
using Game.Core.Detector;
using Game.Meta;
using Game.Services.Inputs.Microservice;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;
using UnityEngine;

namespace Game.Core.Cells
{
    public class CellAddDetector : MonoBehaviour
    {
        public SimpleClickCallback<CellAddDetector> clickCallback;
        private InputObservingMicroservice _inputObservingMicroservice;
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += ManagerSceneAwakeMicroServiceSession;
        }

        private void ManagerSceneAwakeMicroServiceSession()
        {
            _inputObservingMicroservice = SessionStateManager.Instance.Container.Resolve<InputObservingMicroservice>();
        }
        internal void OnClick() 
        {
            clickCallback.OnClick?.Invoke(this);
        }
        internal void OnSelect() 
        {
            clickCallback.OnClick?.Invoke(this);
        }
    }
}