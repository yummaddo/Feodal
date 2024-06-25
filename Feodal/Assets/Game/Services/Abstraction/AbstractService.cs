using System;
using System.Collections;
using System.Threading.Tasks;
using Game.Utility;
using UnityEngine;

namespace Game.Services.Abstraction
{
    public abstract class AbstractService : AbstractServiceBoot
    {
        internal SessionLifeStyleManager LifeStyleManager;
        protected virtual void Awake()
        {
            LifeStyleManager = SessionLifeStyleManager.Instance;
            SessionLifeStyleManager.AddLifeIteration(OnAwakeCaller, SessionLifecycle.OnSceneAwakeSession);
            SessionLifeStyleManager.AddLifeIteration(OnStartCaller, SessionLifecycle.OnSceneStartSession);
        }
        private async Task OnAwakeCaller(IProgress<float> progress)
        {
            Debugger.Logger($"[Service]  {this.GetType().Name}",  ContextDebug.Session,Process.Initial);
            SessionLifeStyleManager.Instance.ServiceLocator.RegisterInstance(this.GetType(), this);
            await  OnAwake(progress);
        }
        private async Task OnStartCaller(IProgress<float> progress)
        {
            await  OnStart(progress);
        }
    }
}