using Game.Services.Abstraction.MicroService;
using Game.Services.Inputs;

namespace Game.Services.Control.Microservice
{
    public class CameraMovementMicroservice : AbstractMicroservice<ControlService>
    {
        public InputService inputService;

        protected override void OnAwake()
        {
            inputService = StateManager.Container.Resolve<InputService>();
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