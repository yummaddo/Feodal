using System;
using System.Collections.Generic;
using Game.CallBacks;
using Game.CallBacks.CallbackClick.Button;
using Game.Cells;
using Game.DataStructures;
using Game.DataStructures.Abstraction;
using Game.DataStructures.Technologies.Abstraction;
using Game.DataStructures.Trades;
using Game.Typing;
using Game.UI.Menu;
using Game.UI.Menu.ResourceListMenu;
using Game.UI.Menu.TechnologyMenu;

namespace Game.Services.ProxyServices
{
    public static class Porting
    {
        /// <summary>
        /// Provides a mapping between types and ports. This utility class facilitates the retrieval of specific ports based on the provided type.
        /// </summary>
        private static readonly Dictionary<Type, Port> Ports = new Dictionary<Type, Port>()
        {
            // SimpleClickCallback<Cell>
            // CellProvider
            {typeof(CellAddDetector), Port.Port1},
            {typeof(CellUpdatedDetector), Port.Port2},
            {typeof(CellResourceFarmer), Port.Port3},
            //
            {typeof(ResourceTempedCallBack), Port.Port3},
            {typeof(TechnologyTempedCallBack), Port.Port3},
            // SimpleCellResourcePackagingCallBack
            // SimpleMenuTypesCloseCallBack
            {typeof(CellMap), Port.Port1},
            {typeof(CellState), Port.Port1},
            //
            {typeof(IResource), Port.Port2},
            {typeof(ITechnologyStore), Port.Port2},
            //
            {typeof(UIResourceListElement), Port.TradeResource},
            {typeof(UITechnologyListElement), Port.TradeResource},
            //
            {typeof(UITradeMenu), Port.Port1},
            {typeof(UITechnologyMenu), Port.Port1},

            {typeof(UIMenuContainer), Port.Port2},
            {typeof(UIMenuResource), Port.Port2},
            {typeof(UIMenuBuilding), Port.Port2},
            //
            {typeof(ButtonExitMenuCallBack), Port.ButtonExitMenu},
            {typeof(ButtonOpenMenuCallBack), Port.ButtonOpenMenu},
            //
            {typeof(BuildingTrade), Port.Trade1},
            {typeof(SeedTrade), Port.Trade2},
            {typeof(ResourceTrade), Port.Trade3},
            {typeof(TechnologyTrade), Port.Trade4},
        };

        private static readonly Dictionary<TradeCallBackResult, Port> PortTrades = new Dictionary<TradeCallBackResult, Port>()
        {
            {TradeCallBackResult.Successfully, Port.TradeSuccessfully},
            {TradeCallBackResult.Failed, Port.TradeFailed},
        };
        /// <summary>
        /// Retrieves the port associated with the specified type.
        /// </summary>
        /// <typeparam name="TValue">The type for which to retrieve the port.</typeparam>
        /// <returns>The port associated with the specified type, or <see cref="Port.PortDefault"/> if no specific port is found.</returns>
        public static Port Type<TValue>()
        {
            if (Ports.TryGetValue(typeof(TValue), out var port))
                return port;
            return Port.PortDefault;
        }
        public static Port Type(TradeCallBackResult callBackResult) => PortTrades[callBackResult];
    }
}