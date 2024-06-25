using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Simple;
using Game.Services.InputServices.Microservice;
using Game.Services.ProxyServices;
using UnityEngine;

namespace Game.Cells
{
    public class CellUpdatedDetector : MonoBehaviour, IDetector
    {
        private InputObservingMicroservice _inputObservingMicroservice;
        public List<GameObject> objectsWithCollider;
        public Cell cell;
        public SimpleCellCallBack clickCallback;
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(ManagerSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
            if (SessionLifeStyleManager.Instance.IsMicroServiceSessionInit) 
                ManagerSceneAwakeMicroServiceSession();
        }
        public void Init(Cell cellInjection)=> this.cell = cellInjection;
        private Task ManagerSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            ManagerSceneAwakeMicroServiceSession();
            return Task.CompletedTask;
        }
        private void ManagerSceneAwakeMicroServiceSession(){
            _inputObservingMicroservice = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<InputObservingMicroservice>();
        }

        public void OnSelect()
        {
            if (!cell.IsBaseState)
                clickCallback.OnCallBackInvocation?.Invoke(Port.SelectionCellBase,cell);
            else
                clickCallback.OnCallBackInvocation?.Invoke(Port.SelectionCellBase,cell);
        }
        public void OnClick()
        {
            if (cell.IsBaseState)
                clickCallback.OnCallBackInvocation?.Invoke(Porting.Type<CellUpdatedDetector>(),cell);
            else
                clickCallback.OnCallBackInvocation?.Invoke(Porting.Type<CellResourceFarmer>(),cell);
        }
    }
}