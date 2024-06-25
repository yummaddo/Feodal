using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Simple;
using Game.Services.InputServices.Microservice;
using Game.Services.ProxyServices;
using UnityEngine;

namespace Game.Cells
{
    public class CellAddDetector : MonoBehaviour, IDetector
    {
        public SimpleCellAddDetectorCallBack clickCallback;
        public List<GameObject> objectsWithCollider;
        private InputObservingMicroservice _inputObservingMicroservice;
        private InputObservingMicroservice _creation;
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(ManagerSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
            if (SessionLifeStyleManager.Instance.IsMicroServiceSessionInit) ManagerSceneAwakeMicroServiceSession();
        }

        private Task ManagerSceneAwakeMicroServiceSession(IProgress<float> arg)
        {
            ManagerSceneAwakeMicroServiceSession();
            return Task.CompletedTask;
        }
        private void ManagerSceneAwakeMicroServiceSession()
        {
            _inputObservingMicroservice = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<InputObservingMicroservice>();
            _creation = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<InputObservingMicroservice>();
        }
        public void OnClick() 
        {
            clickCallback.OnCallBackInvocation?.Invoke(Porting.Type<CellAddDetector>(),this);
        }
        public void OnSelect() 
        {
            // clickCallback.OnClick?.Invoke(this);
        }
    }
}