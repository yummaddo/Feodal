using System.Collections.Generic;
using Game.Core.Abstraction;
using UnityEngine;

namespace Game.Services.Storage
{
    [System.Serializable]
    public abstract class Temp<TEncoded,TEncodedIdentifier,TData>
    {
        internal Temp()
        {
            ResourceData = new Dictionary<TEncoded, TData>();
            ResourceDataByIdentifier = new Dictionary<TEncodedIdentifier, TEncoded>();
            ResourceDataAmountByIdentifier = new Dictionary<TEncodedIdentifier, TData>();
            ResourceViewIndex = new Dictionary<TEncodedIdentifier, int>();
            ResourceViews = new List<View>();
        }
        protected Dictionary<TEncoded, TData> ResourceData { get; set; }
        protected Dictionary<TEncodedIdentifier, TEncoded> ResourceDataByIdentifier { get; set; } 
        protected Dictionary<TEncodedIdentifier, TData> ResourceDataAmountByIdentifier { get; set; }
        protected Dictionary<TEncodedIdentifier, int> ResourceViewIndex { get; set; }
        [field:SerializeField] protected List<View> ResourceViews { get; set; }
        protected abstract TData SumAmounts(TData a, TData b);
        protected abstract TEncodedIdentifier GetIdentifierByEncoded(TEncoded encoded);
        internal Dictionary<TEncoded, TData> GetAllResourceData => ResourceData;
        internal TData GetAmount(TEncodedIdentifier identifier)
        {
            return ResourceDataAmountByIdentifier[identifier];
        }
        internal bool Contains(TEncodedIdentifier identifier)
        {
            if (ResourceDataByIdentifier.ContainsKey(identifier))
            {
                return true;
            }

            return false;
        }
        internal bool Contains(TEncoded encoded)
        {
            if (ResourceDataByIdentifier.ContainsKey(GetIdentifierByEncoded(encoded)))
            {
                return true;
            }

            return false;
        }
        public void AddAmount(TEncodedIdentifier identifier, TData amount)
        {
            if (ResourceDataByIdentifier.ContainsKey(identifier))
            {

                ResourceDataAmountByIdentifier[identifier] = SumAmounts(ResourceDataAmountByIdentifier[identifier], amount);
                ResourceViews[ResourceViewIndex[identifier]].value = ResourceDataAmountByIdentifier[identifier] ;
            }
            else
            {
                Initialization(ResourceDataByIdentifier[identifier], amount);
            }
        }
        public void Initialization(TEncoded encoded, TData amount)
        {
            Debugger.Logger($"{encoded.ToString()} with amount {amount.ToString()} add to temp");
            var identifier = GetIdentifierByEncoded(encoded);
            ResourceData.Add(encoded, amount);
            ResourceDataByIdentifier.Add(identifier,encoded);
            ResourceDataAmountByIdentifier.Add(identifier,amount);
            ResourceViewIndex.Add(identifier,ResourceViews.Count);
            ResourceViews.Add(new View(identifier, amount));
        }

        [System.Serializable]
        protected class View
        {
            public TEncodedIdentifier title;
            public TData value;

            public View(TEncodedIdentifier title, TData value)
            {
                this.title = title;
                this.value = value;
            }
        }
    }
}