using Game.CallBacks.CallbackClick.Abstraction;
using Game.CallBacks.CallbackClick.Button;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu;

namespace Game.CallBacks.CallbackClick.Simple
{
    public class SimpleMenuTypesCloseCallBack : SimpleClickCallback<MenuTypes>
    {
        public override void Initialization()
        {
            MenuTypesExitProvider.CallBackTunneling<ButtonExitMenuCallBack>(this);
            MenuTypesExitProvider.CallBackTunneling<UIMenuContainer>(this);
        }
    }
}