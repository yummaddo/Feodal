using Game.Core.Abstraction;
using Game.Core.DataStructures;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.ClickCallback.Simple;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;
using UnityEngine;

namespace Game.Services.Proxies.ClickCallback.Button
{
    public class ButtonResourceCallBack : ButtonClickCallback<IResource>
    {
        [SerializeField] private SimpleMenuTypesCloseCallBack callBack;
        [SerializeField] private MenuTypes menuTypesToClose = MenuTypes.ContainerMenu;
        [SerializeField] private bool isUniversal = false; 
        [SerializeField] private Resource resource;
        
        protected override void OnButtonClick()
        {
            callBack.OnCallBackInvocation?.Invoke(Porting.Type<ButtonExitMenuCallBack>(), menuTypesToClose);
        }
        public override Port GetPort()
        {
            return Porting.Type<IResource>();
        }
        
        public override void Initialization()
        {
            if (isUniversal)
            {
                UniversalResourceProvider.CallBackTunneling<IResource>( this);
            }
            else
            {
                ResourceProvider.CallBackTunneling<IResource>( this);
            }
            StatusInit = true;
             DataInitialization(resource.Data);
        }
    }
}