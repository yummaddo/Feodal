using System;
using Game.Core.Cell;
using Game.Meta;
using Game.Services.Abstraction.MicroService;
using Game.Services.Inputs;
using UnityEngine;

namespace Game.Services.CellControling.Microservice
{
    public class CellCreateMicroservice : AbstractMicroservice<CellControlling>
    {
        
        public InputService serviceInput;
        public bool createActive;
        public event Action OnServiceCellCreateColed;
        private void ServiceOnCellCreateColed() => OnServiceCellCreateColed?.Invoke();

        protected override void OnAwake()
        {
            serviceInput = SessionStateManager.Instance.Container.Resolve<InputService>();
            serviceInput.OnClickedByAddCellObject += ServiceInputClickedByAddCellObject;
        }

        private void ServiceInputClickedByAddCellObject(GameObject addSign, CellAddDetector detector)
        {
            Debugger.Logger($"Cell:{detector.link.cell} In Direction: {detector.link.direct}");
            OnServiceCellCreateColed?.Invoke();
        }

        protected override void OnStart()
        {
        }

        protected override void ReStart()
        {
        }

        protected override void Stop()
        {
        }

    }
}