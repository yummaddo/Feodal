using System;
using System.Collections.Generic;
using Game.Core.Detector;
using Game.Meta;
using Game.Services.Inputs.Microservice;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.ClickCallback.Simple;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core.Cells
{
    public class CellUpdatedDetector : MonoBehaviour
    {
        private InputObservingMicroservice _inputObservingMicroservice;
        public List<GameObject> objectsWithCollider;
        public Cell cell;
        public SimpleCellCallBack clickCallback;
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += ManagerSceneAwakeMicroServiceSession;
            if (SessionStateManager.Instance.IsMicroServiceSessionInit)
            {
                ManagerSceneAwakeMicroServiceSession();
            }
        }
        private void OnDestroy()
        {
            _inputObservingMicroservice.DeleteUpdateDetectors(objectsWithCollider);
        }
        
        private void ManagerSceneAwakeMicroServiceSession()
        {
            _inputObservingMicroservice = SessionStateManager.Instance.ServiceLocator.Resolve<InputObservingMicroservice>();
            _inputObservingMicroservice.AddDetector(this);
        }
        public void OnSelect()
        {
            if (!cell.IsBaseState)
            {
                
            }
            else
            {
                
            }
        }
        public void OnClick()
        {
            if (cell.IsBaseState)
            {
                clickCallback.OnCallBackInvocation?.Invoke(Porting.Type<CellUpdatedDetector>(),cell);
            }
            else
            {
                clickCallback.OnCallBackInvocation?.Invoke(Porting.Type<CellResourceFarmer>(),cell);
            }
        }
    }
}