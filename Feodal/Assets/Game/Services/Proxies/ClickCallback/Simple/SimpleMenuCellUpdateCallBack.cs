using Game.Core.Abstraction;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;

namespace Game.Services.Proxies.ClickCallback.Simple
{
    public class SimpleMenuCellUpdateCallBack : SimpleClickCallback<ICellContainer>
    {
        public override void Initialization()
        {
            MenuCellUpdateProvider.CallBackTunneling(this);
        }
    }
}