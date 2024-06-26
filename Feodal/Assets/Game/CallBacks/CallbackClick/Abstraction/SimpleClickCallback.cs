using System;
using System.Threading.Tasks;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Utility;
using UnityEngine;

namespace Game.CallBacks.CallbackClick.Abstraction
{
    public abstract class SimpleClickCallback<TData> : MonoBehaviour, IClickCallback<TData>
    {
        // public Action<TD,TData> OnClick { get; set; } = data =>
        //     
        public GameObject TargetObject { get; set; }
        protected bool StatusIteration = false;
        public bool IsInit { get; set; } = false;

        protected virtual void Awake()
        {
            StatusIteration = false;
            SessionLifeStyleManager.AddLifeIteration(AwakeButton, SessionLifecycle.OnSceneAwakeSession);
        }
        private void OnEnable()
        {
            if (!StatusIteration)
            {
                Initialization(this.gameObject);
            }
        }
        private Task AwakeButton(IProgress<float> progress)
        {
            try
            {
                StatusIteration = true;
                Initialization(this.gameObject);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                // ignored
            }
            return Task.CompletedTask;
        }

        protected void Initialization(GameObject targetGameObject)
        {
            TargetObject = targetGameObject;
            Initialization();
        }
        /// <summary>
        /// Tunnelling init
        /// </summary>
        public abstract void Initialization();
        public Action<Port, TData> OnCallBackInvocation { get; set; } = (type, data) =>
        {
            Debugger.Logger($"{type}: {typeof(TData)}, {data.ToString()}",  ContextDebug.Session,Process.Action);
        };
    }
}