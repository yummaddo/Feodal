using Game.CallBacks.CallbackClick.Abstraction;
using Game.CallBacks.CallbackClick.Simple;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.UI.Abstraction;
using Game.UI.Menu;

namespace Game.CallBacks.CallbackClick.Button
{

    public class ButtonListContainerElementCallBack : ButtonClickCallback<IUICellContainerElement>
    {
        public SimpleMenuTypesCloseCallBack callBack;
        public MenuTypes menuTypesToClose = MenuTypes.BuildingMenu;
        public override void Initialization()
        {
            UICellContainerElementProvider.CallBackTunneling<UIMenuBuilding>(this);
            StatusInit = true;
        }
        protected override void BeforeButtonClick()
        {
            callBack.OnCallBackInvocation?.Invoke(Porting.Type<ButtonExitMenuCallBack>(), menuTypesToClose);
        }
        public override Port GetSenderPort()
        {
            return Porting.Type<UIMenuBuilding>();
        }
    }
}