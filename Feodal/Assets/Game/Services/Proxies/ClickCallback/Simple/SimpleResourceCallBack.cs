using Game.Core.Abstraction;
using Game.Core.Cells;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;

namespace Game.Services.Proxies.ClickCallback.Simple
{
    public class SimpleResourceCallBack : SimpleClickCallback<CellResourcePackaging>
    {
        public override void Initialization()
        {
            ResourceFarmProvider.CallBackTunneling(this);
        }
    }
}