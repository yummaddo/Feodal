using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Conditions.Abstraction.Trades;
using Game.Core.DataStructures.Conditions.TradesConditions;
using Game.Services.Storage.Abstraction;
using Game.Services.Storage.TechnologyRepositories;

namespace Game.Services.Storage.ResourcesRepository
{
    [System.Serializable]
    public class ResourceTemp : Temp<ResourceEncoded,string, long>
    {
        internal Dictionary<string, IResource> Technologies;
        internal Dictionary<string, ITradeSeedCondition> SeedTrades;
        internal Dictionary<string, ITradeResourceTechnologyCondition> ResourceTechnologyTrade;
        internal Dictionary<string, ITradeResourceCondition> ResourceTrade;
        
        internal void InjectResource(List<Resource> technologies, ResourceRepository repository)
        {
            Technologies = new Dictionary<string, IResource>();
            foreach (var rTechnology in technologies)
            {
                var temped = rTechnology.Data; 
                temped.Temp = this;
                temped.Repository = repository;
                Technologies.Add(temped.Title, temped);
            }
        }
        internal void InjectTrade(List<ConditionTradeSeed> trade, TechnologyRepository repository)
        {
            SeedTrades = new Dictionary<string, ITradeSeedCondition>();
            foreach (var rTechnology in trade)
            {
                var temped = rTechnology.Data; 
                temped.ResourceTemp = this;
                temped.TechnologyTemp = repository.temp;
                SeedTrades.Add(rTechnology.Data.ConditionName, temped);
            }
        }
        internal void InjectTrade(List<ConditionTradeResourceTechnology> trade, TechnologyRepository repository)
        {
        }
        internal void InjectTrade(List<ConditionTradeResourceResourceAmount> trade, TechnologyRepository repository)
        {
        }
        internal override void Injection()
        {
            base.Injection();
        }
        protected override long SumAmounts(long a, long b)
        {
            return a + b;
        }
        protected override string GetIdentifierByEncoded(ResourceEncoded encoded)
        {
            return encoded.Title;
        }
        internal void ProvideAddAmounts(IResource resource, int amount)
        {
            DataByIdentifier[resource.Title] += amount;
        }
        internal void ProvideGetAmounts(IResource resource, int amount)
        {
            DataByIdentifier[resource.Title] -= amount;
        }
        internal void ProvideTrade(IResource resource, int amount)
        {
        }
        internal void CanTrade(IResource resource, int amount)
        {
        }
        internal void CanGetAmounts(IResource resource, int amount)
        {
        }
        internal void  CanAddAmounts(IResource resource, int amount)
        {
        }
        // internal void GetAmounts(IResource resource, int amount)
        // {
        //     DataByIdentifier[resource.Title] -= amount;
        // }
        internal ResourceTemp(IIdentifier<string, ResourceEncoded> identifier) : base(identifier)
        {
            
        }
    }
}