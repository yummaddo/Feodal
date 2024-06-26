using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
    public abstract class UIElementOnEnable : MonoBehaviour
    {
        public bool isInit = false;
        private void Awake()
        {
            OnAwake();
            AwakeLifeIterationInjection();
        }
        private Task UpdateOnInit(IProgress<float> progress)
        {
            isInit = true;
            UpdateOnInit();
            return Task.CompletedTask;
        }
        private void OnEnable()
        {
            if (!isInit)
            {
                UpdateOnInit();
                OnEnableSProcess();
            }
            else
            {
                OnEnableSProcess();
            }
        }

        public virtual void AwakeLifeIterationInjection(
            SessionLifecycle sessionLifecycle = SessionLifecycle.OnSceneAwakeClose)
        {
            SessionLifeStyleManager.AddLifeIteration(UpdateOnInit,sessionLifecycle);
        }
        public abstract void OnEnableSProcess();
        public abstract void OnAwake();
        public abstract void UpdateOnInit();
    }
}