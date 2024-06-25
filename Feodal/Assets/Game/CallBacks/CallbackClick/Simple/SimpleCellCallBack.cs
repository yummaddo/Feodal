using Game.CallBacks.CallbackClick.Abstraction;
using Game.Cells;
using Game.Services.ProxyServices.Providers;

namespace Game.CallBacks.CallbackClick.Simple
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