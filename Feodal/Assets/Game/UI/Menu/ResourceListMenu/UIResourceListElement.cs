using System;
using System.Threading.Tasks;
using Game.CallBacks;
using Game.CallBacks.CallbackClick.Button;
using Game.DataStructures;
using Game.DataStructures.Abstraction;
using Game.DataStructures.UI;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers.DatabaseProviders;
using Game.Services.StorageServices;
using Game.Typing;
using Game.UI.Abstraction;
using UnityEngine;
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
        public Resource universal;
        public ButtonListResourceElementCallBack buttonResourceCallBack;
        public override void OnEnableSProcess()
        {
        }
        public override void OnAwake()
        {
        }
        public override void UpdateOnInit()
        {
            Proxy.Connect<DatabaseResourceProvider,ResourceTempedCallBack,ResourceTempedCallBack>(SomeResourceUpdate);
            var store = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<StorageService>();
            var temp = store.GetResourceTemp();
            if (type == ResourceType.Universal)
                UpdateValue(temp.GetAmount(universal.title));
        }
        public void UpdateData(UIResource newResource)
        {
            UiResource = newResource.Data;
            type = newResource.resource.Data.Type;
            buttonResourceCallBack.DataInitialization(this);
            UpdateData();
        }
        public void UpdateData(UISeed newResource)
        {
            UiResource = newResource.Data;
            type = newResource.resource.Data.Type;
            buttonResourceCallBack.DataInitialization(this);
            UpdateData();
        }
        private void SomeResourceUpdate(Port arg1, ResourceTempedCallBack arg2)
        {
            if (type == ResourceType.Universal)
            {
                if (arg2.Resource.Title == universal.title)
                    UpdateValue(arg2.Value);
            }
            else
            {
                if (arg2.Resource.Title == UiResource.Resource.Title)
                    UpdateValue(arg2.Value);
            }
        }
        private void UpdateData()
        {
            image.sprite = UiResource.ResourceImage;
            
            if(type!= ResourceType.Universal)
                rare.sprite = UiResource.ResourceRareImage.GetSprite(UiResource.Resource.Rare);
            
            if(type!= ResourceType.Universal)
                title.text = UiResource.Title;
            
            var valueAmount = UiResource.Resource.Temp.GetAmount(UiResource.Resource.Title);
            if (value) value.text = valueAmount.ToString();
        }
        public void UpdateValue(long valueElement)
        {
            if (value) value.text = valueElement.ToString();
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