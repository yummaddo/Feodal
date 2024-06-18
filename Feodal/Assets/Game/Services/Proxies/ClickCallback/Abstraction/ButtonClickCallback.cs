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
        private void Awake()
        {
            Initialization();
            if (!button) button = transform.GetComponent<UnityEngine.UI.Button>();
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

        private void ButtonClick()
        {
            OnClick?.Invoke(Data);
        }
        public void DataInitialization(TData data) => Data = data;
        public Action<TData> OnClick { get; set; } = data =>
            Debugger.Logger($"ButtonClickCallback: {typeof(TData)}, {data.ToString()}",  ContextDebug.Session,Process.Action);

        public virtual void Initialization()
        {
            StatusInit = true;
        }
    }
}