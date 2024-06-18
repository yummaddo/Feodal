using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;

namespace Game.Services.Proxies.ClickCallback.Button
{
    public class ButtonExitMenuCallBack: ButtonClickCallback<MenuTypes>
    {
        public override void Initialization()
        {
            MenuExitProvider.CallBackTunneling(this);
            StatusInit = true;
        }
    }
}