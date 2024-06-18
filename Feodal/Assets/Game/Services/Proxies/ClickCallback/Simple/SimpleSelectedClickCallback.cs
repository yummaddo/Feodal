using Game.Core;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;

namespace Game.Services.Proxies.ClickCallback.Simple
{
    public class SimpleSelectedClickCallback : SimpleClickCallback<Cell>
    {
        public override void Initialization()
        {
            CellSelectProvider.CallBackTunneling(this);
        }
    }
}