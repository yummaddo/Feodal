﻿using Game.Core;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;

namespace Game.Services.Proxies.ClickCallback.Simple
{
    public class SimpleCellCallBack : SimpleClickCallback<Cell>
    {
        public override void Initialization()
        {
            CellUpdateProvider.CallBackTunneling(this);
        }
    }
}