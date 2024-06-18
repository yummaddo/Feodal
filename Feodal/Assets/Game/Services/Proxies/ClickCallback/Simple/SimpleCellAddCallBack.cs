using Game.Core.Cells;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;

namespace Game.Services.Proxies.ClickCallback.Simple
{
    public class SimpleCellAddCallBack : SimpleClickCallback<CellAddDetector>
    {
        public override void Initialization()
        {
            CellAddProvider.CallBackTunneling(this);
        }
    }
}