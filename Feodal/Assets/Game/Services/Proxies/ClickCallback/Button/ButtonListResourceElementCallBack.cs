using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;
using Game.UI.Menu.ResourceListMenu;

namespace Game.Services.Proxies.ClickCallback.Button
{
    public class ButtonListResourceElementCallBack : ButtonClickCallback<UIResourceListElement>
    {
        public override void Initialization()
        {
            StatusInit = true;
            UIListResourceElementProvider.CallBackTunneling<ButtonOpenMenuCallBack>(this);
        }
        protected override void OnButtonClick()
        {
            
        }
        public override Port GetPort()
        {
            return Porting.Type<UIResourceListElement>();
        }
    }
}