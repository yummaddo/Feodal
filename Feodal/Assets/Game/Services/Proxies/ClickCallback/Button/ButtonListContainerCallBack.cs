using Game.Core.Abstraction.UI;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.ClickCallback.Simple;
using Game.Services.Proxies.Providers;
using Game.UI.Menu;

namespace Game.Services.Proxies.ClickCallback.Button
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

        protected override void OnButtonClick()
        {
            callBack.OnClick?.Invoke(Porting.Type<ButtonExitMenuCallBack>(), menuTypesToClose);
        }

        public override Port GetPort()
        {
            return Porting.Type<UIMenuBuilding>();
        }
    }
}