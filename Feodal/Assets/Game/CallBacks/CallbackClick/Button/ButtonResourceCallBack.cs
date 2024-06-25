using Game.CallBacks.CallbackClick.Abstraction;
using Game.CallBacks.CallbackClick.Simple;
using Game.DataStructures;
using Game.DataStructures.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu;
using UnityEngine;

namespace Game.CallBacks.CallbackClick.Button
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