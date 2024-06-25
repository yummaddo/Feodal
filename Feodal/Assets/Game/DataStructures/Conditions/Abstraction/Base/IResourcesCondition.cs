namespace Game.DataStructures.Conditions.Abstraction.Base
{
    public interface IResourcesCondition<TResourceTrade> : ICondition
    {
        public TResourceTrade ConnectedToDependency { get; set; }
    }
}