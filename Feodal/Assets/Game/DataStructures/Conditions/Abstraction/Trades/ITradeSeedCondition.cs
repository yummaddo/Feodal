using Game.DataStructures.Trades;

namespace Game.DataStructures.Conditions.Abstraction.Trades
{
    public interface ITradeSeedCondition : ICondition
    { 
        public SeedTrade ConnectedToDependency { get; set; }
    }
}