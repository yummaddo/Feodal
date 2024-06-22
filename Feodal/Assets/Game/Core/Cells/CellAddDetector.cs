using System;
using System.Collections.Generic;
using Game.Core.Detector;
using Game.Meta;
using Game.Services.Inputs.Microservice;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.ClickCallback.Simple;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;
using UnityEngine;

namespace Game.Core.Cells
{
    public class CellAddDetector : MonoBehaviour
    {
        public SimpleCellAddDetectorCallBack clickCallback;
        
        private InputObservingMicroservice _inputObservingMicroservice;
        private InputObservingMicroservice _creation;

        public List<GameObject> objectsWithCollider;
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += ManagerSceneAwakeMicroServiceSession;
            if (SessionStateManager.Instance.IsMicroServiceSessionInit)
            {
                ManagerSceneAwakeMicroServiceSession();
            }
        }

        private void ManagerSceneAwakeMicroServiceSession()
        {
            _inputObservingMicroservice = SessionStateManager.Instance.Container.Resolve<InputObservingMicroservice>();
            _creation = SessionStateManager.Instance.Container.Resolve<InputObservingMicroservice>();
            
            _inputObservingMicroservice.AddDetector(this);
        }

        private void OnDestroy()
        {
            _inputObservingMicroservice.DeleteDetector(objectsWithCollider);
        }

        internal void OnClick() 
        {
            clickCallback.OnClick?.Invoke(Porting.Type<CellAddDetector>(),this);
            
        }
        internal void OnSelect() 
        {
            // clickCallback.OnClick?.Invoke(this);
        }
    }
}