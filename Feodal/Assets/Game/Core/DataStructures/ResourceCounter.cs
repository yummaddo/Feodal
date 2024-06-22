﻿using System.Collections.Generic;
using Game.Core.DataStructures.Trades;

namespace Game.Core.DataStructures
{
    [System.Serializable]
    public class ResourceCounter
    {
        public Resource resource;
        public int value;

        public ResourceCounter(Resource resource, int value)
        {
            this.resource = resource;
            this.value = value;
        }
        public override string ToString()
        {
            return $"{resource.title}{value}";
        }
    }
    [System.Serializable]
    public class ResourceCounterStage
    {
        public ResourceTrade resource;
        public ResourceCounterStage(ResourceTrade resourceTrades)
        {
            resource = resourceTrades;
        }
    }
}