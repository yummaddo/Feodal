using Game.Core.Abstraction;
using Game.Core.Cells;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;

namespace Game.Services.Proxies.ClickCallback.Simple
{
    public class SimpleCellResourcePackagingCallBack : SimpleClickCallback<CellResourcePackaging>
    {
        public override void Initialization()
        {
            CellResourcePackagingProvider.CallBackTunneling<CellResourceFarmer>(this);
        }
    }
}