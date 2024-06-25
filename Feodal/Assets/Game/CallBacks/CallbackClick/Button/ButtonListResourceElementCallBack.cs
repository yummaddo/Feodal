using Game.CallBacks.CallbackClick.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu.ResourceListMenu;

namespace Game.CallBacks.CallbackClick.Button
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