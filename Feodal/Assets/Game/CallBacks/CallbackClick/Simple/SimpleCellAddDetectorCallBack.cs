using Game.CallBacks.CallbackClick.Abstraction;
using Game.Cells;
using Game.Services.ProxyServices.Providers;

namespace Game.CallBacks.CallbackClick.Simple
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