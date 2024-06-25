using System;
using System.Threading.Tasks;
using Game.Utility;
using UnityEngine;

namespace Game.Services.Abstraction
{
    public abstract class AbstractMicroservice<TService> : AbstractServiceBoot
    
       where TService : AbstractService
    {
        protected TService Service;
        protected virtual void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(SceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
            SessionLifeStyleManager.AddLifeIteration(SceneStartMicroServiceSession, SessionLifecycle.OnSceneStartSession);
        }
        private async Task SceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            Debugger.Logger($"[Microservice] {this.GetType().Name}",  ContextDebug.Initialization, Process.Initial);
            SessionLifeStyleManager.Instance.ServiceLocator.RegisterInstance(this.GetType(), this);
            GetParentService();
            await OnAwake(progress);
        }
        private async Task SceneStartMicroServiceSession(IProgress<float> progress)
        {
            GetParentService();
            await OnStart(progress);
        }
        private void GetParentService()
        {
            var stateManager = SessionLifeStyleManager.Instance;
            Service = stateManager.ServiceLocator.Resolve<TService>();
        }
    }
}