using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;

namespace Game.Services.Proxies.ClickCallback.Simple
{
    public class SimpleMenuTypesOpenCallBack : SimpleClickCallback<MenuTypes>
    {
        public override void Initialization()
        {
            // MenuTypesOpenProvider.CallBackTunneling(Port.Port2,this);
        }
    }
}