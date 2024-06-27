using System;
using Game.CallBacks;
using Game.DataStructures.Abstraction;
using Game.DataStructures.UI;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers.DatabaseProviders;
using Game.Typing;
using Game.UI.Abstraction;
using UnityEngine.UI;

namespace Game.UI.Menu.ResourceListMenu
{

    public class UIResourceListElement : UIElementOnEnable
    {
        public IUIResource UiResource;
        public ResourceType type;
        public Image image;
        public Image rare;
        public Text title;
        public Text value;
        public Button click;
        
        internal event Action<UIResourceListElement> OnClickEvent;
        
        protected override void OnEnableSProcess() { }
        protected override void OnAwake() { }
        
        private void OnClick() => OnClickEvent?.Invoke(this);
        protected override void UpdateOnInit()
        {
            isInit = true;
            Proxy.Connect<DatabaseResourceProvider,ResourceTempedCallBack,ResourceTempedCallBack>(SomeResourceUpdate);
            click.onClick.AddListener(OnClick);
        }
        public void UpdateData(UIResource newResource)
        {
            UiResource = newResource.Data;
            type = newResource.resource.Data.Type;
            UpdateData();
        }
        public void UpdateData(UISeed newResource)
        {
            UiResource = newResource.Data;
            type = newResource.resource.Data.Type;
            UpdateData();
        }
        private void SomeResourceUpdate(Port arg1, ResourceTempedCallBack arg2)
        {
            if (arg2.Resource.Title == UiResource.Resource.Title) 
                UpdateValue(arg2.Value);
        }
        private void UpdateData()
        {
            image.sprite = UiResource.ResourceImage;
            rare.sprite = UiResource.ResourceRareImage.GetSprite(UiResource.Resource.Rare);
            var valueAmount = UiResource.Resource.Temp.GetAmount(UiResource.Resource.Title);
            if (value) 
                value.text = valueAmount.ToString();
            if (title)
                title.text = UiResource.Resource.Title;
        }
        private void UpdateValue(long valueElement)
        {
            if (value) 
                value.text = valueElement.ToString();
        }
        public bool TryUpdate(IResource callBackResource, long callBackValue)
        {
            if (UiResource.Resource.Title == callBackResource.Title)
            {
                value.text = callBackValue.ToString();
                return true;
            }
            return false;
        }
    }
}