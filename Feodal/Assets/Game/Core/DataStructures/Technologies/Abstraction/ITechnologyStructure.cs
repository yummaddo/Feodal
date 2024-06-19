using System.Collections.Generic;
using Game.Core.DataStructures.Technologies.Base;

namespace Game.Core.DataStructures.Technologies.Abstraction
{
    public interface ITechnologyStructure
    {
        public List<TradeBuildTechnology> TradeBuildTechnologies  { get; set; }
        public List<TradeResourceTechnology> TradeResourceTechnologies  { get; set; }
        public List<CellTechnology> CellTechnologies  { get; set; }
    }
}