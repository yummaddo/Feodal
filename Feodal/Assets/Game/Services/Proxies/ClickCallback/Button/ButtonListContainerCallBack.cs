using Game.Core.Abstraction.UI;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;

namespace Game.Services.Proxies.ClickCallback.Button
{
    public class ButtonListContainerCallBack : ButtonClickCallback<IUICellContainer>
    {
        public override void Initialization()
        {
            CellContainerProvider.CallBackTunneling(this);
            StatusInit = true;
        }
    }
}