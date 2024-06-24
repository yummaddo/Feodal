using Game.Core.Abstraction.UI;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.ClickCallback.Simple;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;

namespace Game.Services.Proxies.ClickCallback.Button
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
        protected override void OnButtonClick()
        {
            callBack.OnCallBackInvocation?.Invoke(Porting.Type<ButtonExitMenuCallBack>(), menuTypesToClose);
        }
        public override Port GetPort()
        {
            return Porting.Type<UIMenuBuilding>();
        }
    }
}