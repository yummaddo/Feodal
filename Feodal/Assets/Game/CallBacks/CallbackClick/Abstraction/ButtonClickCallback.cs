using System;
using System.Threading.Tasks;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Utility;
using UnityEngine;

namespace Game.CallBacks.CallbackClick.Abstraction
{
    public abstract class ButtonClickCallback<TData> : MonoBehaviour, IClickCallback<TData>
    {
        [SerializeField] private UnityEngine.UI.Button button;
        [field:SerializeField] protected TData Data { get; set; }
        protected bool StatusInit = false;
        protected bool StatusIteration = false;
        private Action<Port, TData> _onClick;
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(AwakeButton, SessionLifecycle.OnSceneAwakeSession);
            StatusInit = false;
        }
        private Task AwakeButton(IProgress<float> progress)
        {
            Construct();
            return Task.CompletedTask;
        }
        private void Construct()
        {
            Initialization();
            TargetObject = this.gameObject;
            if (!button)
                button = transform.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.onClick.AddListener(ButtonClick);
            }
            else
            {
                throw new Exception($"no button element on {transform.name}");
            }

            StatusInit = true;
            StatusIteration = true;
        }
        private void OnEnable()
        {
            if (!StatusInit && !StatusIteration)
            {
                Construct();
                StatusInit = true;
            }
        }
        protected abstract void OnButtonClick();
        private void ButtonClick()
        {
            OnButtonClick();
            OnCallBackInvocation?.Invoke(GetPort(),Data);
        }
        public void DataInitialization(TData data) => Data = data;
        public abstract Port GetPort();
        public virtual void Initialization()
        {
            StatusInit = true;
        }
        public Action<Port, TData> OnCallBackInvocation { get; set; } =
            (type,data) => Debugger.Logger($"Button {type} {typeof(TData).Name}, {data.ToString()}",  ContextDebug.Session,Process.Action);
        public bool IsInit { get; set; } = false;
        public GameObject TargetObject { get; set; }
    }
}