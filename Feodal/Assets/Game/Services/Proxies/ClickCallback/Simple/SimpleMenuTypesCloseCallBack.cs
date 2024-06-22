using System.Collections.Generic;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;

namespace Game.Services.Proxies.ClickCallback.Simple
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