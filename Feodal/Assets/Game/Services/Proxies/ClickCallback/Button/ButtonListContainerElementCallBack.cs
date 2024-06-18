using Game.Core.Abstraction.UI;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;

namespace Game.Services.Proxies.ClickCallback.Button
{

    public class ButtonListContainerElementCallBack : ButtonClickCallback<IUICellContainerElement>
    {
        public override void Initialization()
        {
            CellContainerElementProvider.CallBackTunneling(this);
            StatusInit = true;
        }
    }
}