using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;

namespace Game.Services.Proxies.ClickCallback.Simple
{
    public class SimpleMenuOpenCallBack : SimpleClickCallback<MenuTypes>
    {
        public override void Initialization()
        {
            MenuOpenProvider.CallBackTunneling(this);
        }
    }
}