using Game.DataStructures.Trades;

namespace Game.DataStructures.Technologies.Abstraction
{
    public interface ITechnologyStore : ITechnology
    {
        TechnologyTrade Trade { get; set; }
    }
}