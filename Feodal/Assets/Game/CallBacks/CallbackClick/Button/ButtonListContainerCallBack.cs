using Game.CallBacks.CallbackClick.Abstraction;
using Game.CallBacks.CallbackClick.Simple;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.UI.Abstraction;
using Game.UI.Menu;

namespace Game.CallBacks.CallbackClick.Button
{
    
    public class ButtonListContainerCallBack : ButtonClickCallback<IUICellContainer>
    {
        public SimpleMenuTypesCloseCallBack callBack;
        public MenuTypes menuTypesToClose = MenuTypes.ContainerMenu;
        public override void Initialization()
        {
            UICellContainerProvider.CallBackTunneling<UIMenuBuilding>(this);
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