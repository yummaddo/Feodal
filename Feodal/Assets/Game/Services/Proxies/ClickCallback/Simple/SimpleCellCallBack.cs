using Game.Core;
using Game.Core.Cells;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;

namespace Game.Services.Proxies.ClickCallback.Simple
{
    public class SimpleCellCallBack : SimpleClickCallback<Cell>
    {
        public override void Initialization()
        {
            CellProvider.CallBackTunneling<CellAddDetector>(this);
            CellProvider.CallBackTunneling<CellUpdatedDetector>(this);
            CellProvider.CallBackTunneling<CellResourceFarmer>(this);
        }
    }
}