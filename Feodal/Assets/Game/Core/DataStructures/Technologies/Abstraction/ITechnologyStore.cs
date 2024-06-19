using System.Collections.Generic;
using Game.Core.DataStructures.Conditions.Abstraction;
using Game.Core.DataStructures.Trades;

namespace Game.Core.DataStructures.Technologies.Abstraction
{
    public interface ITechnologyStore : ITechnology
    {
        TechnologyTrade Trade { get; set; }
    }
}