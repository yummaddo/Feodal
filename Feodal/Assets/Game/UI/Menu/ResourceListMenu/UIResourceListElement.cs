using System;
using Game.Core.Abstraction;
using Game.Core.Abstraction.UI;
using Game.Core.DataStructures;
using Game.Core.DataStructures.UI.Data;
using Game.Core.Typing;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.Providers;
using Game.Services.Storage;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu.ResourceListMenu
{

    public class UIResourceListElement : MonoBehaviour
    {
        public IUIResource UiResource;
        public ResourceType type;
        public Image image;
        public Image rare;
        public Text title;
        public Text value;
        public Resource universal;
        public ButtonListResourceElementCallBack buttonResourceCallBack;

        private bool _isInit = false;
        private void OnEnable()
        {
            var sessionManager = SessionStateManager.Instance;
            if (sessionManager.IsMicroServiceSessionInit && !_isInit)
            {
                UpdateOnInit();
            }
            else if (!_isInit)
            {
                sessionManager.OnSceneStartSession += UpdateOnInit;
            }
        }

        private void UpdateOnInit()
        {
            _isInit = true;
            Proxy.Connect<DatabaseResourceProvider,ResourceTempedCallBack,ResourceTempedCallBack>(SomeResourceUpdate);
            var store = SessionStateManager.Instance.ServiceLocator.Resolve<StorageService>();
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
        }
        private void UpdateData()
        {
            image.sprite = UiResource.ResourceImage;
            
            if(type!= ResourceType.Universal)
                rare.sprite = UiResource.ResourceRareImage.GetSprite(UiResource.Resource.Rare);
            
            if(type!= ResourceType.Universal)
                title.text = UiResource.Title;
            
            var valueAmount = UiResource.Resource.Temp.GetAmount(UiResource.Resource.Title);
            value.text = valueAmount.ToString();
        }
        public void UpdateValue(long valueElement)
        {
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