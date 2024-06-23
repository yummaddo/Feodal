using System;
using System.Collections.Generic;
using Game.Core;
using Game.Core.Abstraction;
using Game.Core.Cells;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Storage.ResourcesRepository;
using Game.UI.Menu;
using Game.UI.Menu.ResourceListMenu;
using Game.UI.Menu.TechnologyMenu;

namespace Game.Services.Proxies
{
    public static class Porting
    {
        /// <summary>
        /// Provides a mapping between types and ports. This utility class facilitates the retrieval of specific ports based on the provided type.
        /// </summary>
        private static Dictionary<Type, Port> _ports = new Dictionary<Type, Port>()
        {
            // SimpleClickCallback<Cell>
            // CellProvider
            {typeof(CellAddDetector), Port.Port1},
            {typeof(CellUpdatedDetector), Port.Port2},
            {typeof(CellResourceFarmer), Port.Port3},
            
            {typeof(ResourceTempedCallBack), Port.Port3},
            // SimpleCellResourcePackagingCallBack
            // SimpleMenuTypesCloseCallBack
            {typeof(CellMap), Port.Port1},
            //
            {typeof(IResource), Port.Port2},
            //
            {typeof(UIResourceListElement), Port.TradeResource},
            {typeof(UITechnologyListElement), Port.TradeResource},
            
            {typeof(UIMenuContainer), Port.Port2},
            {typeof(UIMenuResource), Port.Port2},
            {typeof(UIMenuBuilding), Port.Port2},
            
            {typeof(ButtonExitMenuCallBack), Port.ButtonExitMenu},
            {typeof(ButtonOpenMenuCallBack), Port.ButtonOpenMenu},

        };
        /// <summary>
        /// Retrieves the port associated with the specified type.
        /// </summary>
        /// <typeparam name="TValue">The type for which to retrieve the port.</typeparam>
        /// <returns>The port associated with the specified type, or <see cref="Port.PortDefault"/> if no specific port is found.</returns>
        public static Port Type<TValue>()
        {
            if (_ports.TryGetValue(typeof(TValue), out var port))
            {
                return port;
            }
            return Port.PortDefault;
        }
    }
    /// <summary>
    /// Enumerates the different ports that can be associated with various types.
    /// </summary>
    public enum Port
    {
        TradeResource,
        
        PortDefault,
        ButtonExitMenu,
        ButtonOpenMenu,
        Port1,
        Port2,
        Port3,
    }
}