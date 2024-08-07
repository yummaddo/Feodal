﻿using System.Collections.Generic;
using Game.DataStructures;
using Game.DataStructures.Conditions;
using Game.DataStructures.Technologies;
using Game.DataStructures.Trades;
using Game.RepositoryEngine.ResourcesRepository;
using Game.RepositoryEngine.TechnologyRepositories;

namespace Game.Services.StorageServices
{
    internal static class StorageServiceConvert
    {
        internal static List<ResourceEncoded> GetEncodedResourcesTemplate(this StorageService service,  List<Resource> resourceList, List<Seed> seeds)
        {
            var resourceEncoded = new List<ResourceEncoded>();
            foreach (var resource in resourceList) resourceEncoded.Add(new ResourceEncoded(resource.Data));
            foreach (var resource in seeds) resourceEncoded.Add(new ResourceEncoded(resource.Data));
            return resourceEncoded;
        }
        internal static List<TechnologyEncoded> GetEncodedTechnologyTemplate(this StorageService service, List<Technology> technologyList)
        {
            var techEncoded = new List<TechnologyEncoded>();
            foreach (var tech in technologyList) techEncoded.Add(new TechnologyEncoded(tech));
            return techEncoded;
        }
        internal static List<ResourceTrade> GetTradeSetTemplate(this StorageService service,List<ConditionTradeResourceAmount> resourceAmounts)
        {
            var list = new List<ResourceTrade>();
            foreach (var amount in resourceAmounts) { list.Add(amount.connectedToDependency); }
            return list;
        }
        internal static List<BuildingTrade> GetTradeSetTemplate(this StorageService service,List<TradeBuildTechnology> buildTechnologies)
        {
            var list = new List<BuildingTrade>();
            foreach (var trade in buildTechnologies) { list.Add(trade.IntoBuild); }
            return list;
        }
        internal static List<SeedTrade> GetTradeSetTemplate(this StorageService service,List<ConditionTradeSeed> tradesSeed)
        {
            var list = new List<SeedTrade>();
            foreach (var trade in tradesSeed) { list.Add(trade.connectedToDependency); }
            return list;
        }
        internal static List<TechnologyTrade> GetTradeSetTemplate(this StorageService service, List<Technology> technologies)
        {
            var list = new List<TechnologyTrade>();
            foreach (var trade in technologies) { list.Add(trade.Data.Trade); }
            return list;
        }
    }
}