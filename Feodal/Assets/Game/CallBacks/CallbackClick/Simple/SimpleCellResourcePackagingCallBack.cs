using Game.CallBacks.CallbackClick.Abstraction;
using Game.Cells;
using Game.Services.ProxyServices.Providers;

namespace Game.CallBacks.CallbackClick.Simple
{
    public class SimpleCellResourcePackagingCallBack : SimpleClickCallback<CellResourcePackaging>
    {
        public override void Initialization()
        {
            CellResourcePackagingProvider.CallBackTunneling<CellResourceFarmer>(this);
        }
    }
}