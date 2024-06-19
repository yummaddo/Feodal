using Game.Core.DataStructures.Trades;

namespace Game.Core.DataStructures.Technologies.Abstraction
{
    public interface ITradeResourceTechnology : ITechnology

    {
    public ResourceTrade ResourceTrade { get; set; }
    }
}