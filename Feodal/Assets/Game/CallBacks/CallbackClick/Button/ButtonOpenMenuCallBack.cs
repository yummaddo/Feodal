using Game.CallBacks.CallbackClick.Abstraction;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu;

namespace Game.CallBacks.CallbackClick.Button
{
    public class ButtonOpenMenuCallBack: ButtonClickCallback<MenuTypes>
    {
        public override void Initialization()
        {
            StatusInit = true;
            MenuTypesOpenProvider.CallBackTunneling<ButtonOpenMenuCallBack>(this);
        }
        protected override void BeforeButtonClick()
        {
            
        }
        public override Port GetSenderPort()
        {
            return Porting.Type<ButtonOpenMenuCallBack>();
        }
    }
}