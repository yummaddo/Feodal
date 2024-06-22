using Game.Core.Cells;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;

namespace Game.Services.Proxies.ClickCallback.Simple
{
    /// <summary>
    /// Port = Cell
    /// </summary>
    public class SimpleCellAddDetectorCallBack : SimpleClickCallback<CellAddDetector>
    {
        public override void Initialization()
        {
            CellAddDetectorProvider.CallBackTunneling<CellAddDetector>(this);
        }
    }
}