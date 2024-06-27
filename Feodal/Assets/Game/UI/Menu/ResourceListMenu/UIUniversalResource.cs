using System;
using System.Threading.Tasks;
using Game.CallBacks;
using Game.DataStructures;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers.DatabaseProviders;
using Game.Services.StorageServices;
using Game.Typing;
using UnityEngine.UI;

namespace Game.UI.Menu.ResourceListMenu
{
    public class UIUniversalResource : UIElementOnEnable
    {
        public Resource resource;
        public Text value;
        public Text title;
        protected override void OnEnableSProcess() { }
        protected override void OnAwake()
        {
            SessionLifeStyleManager.AddLifeIteration(AwakeButton, SessionLifecycle.OnSceneAwakeClose);
        }
        private Task AwakeButton(IProgress<float> progress)
        {
            Proxy.Connect<DatabaseResourceProvider,ResourceTempedCallBack,ResourceTempedCallBack>(SomeResourceUpdate);
            var store = SessionLifeStyleManager.Instance.ServiceLocator.Resolve<StorageService>();
            var temp = store.GetResourceTemp();
            if( title)
                title.text = resource.title;
            UpdateValue(temp.GetAmount(resource.Data.Title));
            return Task.CompletedTask;
        }
        protected override void UpdateOnInit()
        {
            isInit = true;
         }
        private void SomeResourceUpdate(Port port, ResourceTempedCallBack callBack)
        {
            if (callBack.Resource.Title == resource.Data.Title) 
                UpdateValue(callBack.Value);
        }
        private void UpdateValue(long valueElement)
        {
            if (value) 
                value.text = valueElement.ToString();
        }
    }
}