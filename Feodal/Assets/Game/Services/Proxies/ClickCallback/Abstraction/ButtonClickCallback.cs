using System;
using Game.Services.Proxies.Abstraction;
using UnityEngine;

namespace Game.Services.Proxies.ClickCallback.Abstraction
{
    public abstract class ButtonClickCallback<TData> : MonoBehaviour, IClickCallback<TData>
    {
        [SerializeField] private UnityEngine.UI.Button button;
        [field:SerializeField] protected TData Data { get; set; }
        protected bool StatusInit = false;
        private Action<Port, TData> _onClick;
        private void Awake()
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
        }
        private void OnEnable()
        {
            if (!StatusInit)
            {
                Initialization();
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