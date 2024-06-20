using System.Collections.Generic;
using System.Linq;
using Game.Core.Abstraction;
using Game.Meta;
using Game.Services.CellControlling;
using Game.Services.Storage.Abstraction;
using UnityEngine;

namespace Game.Services.Storage
{
    [System.Serializable]
    public class Temp<TEncoded, TEncodedIdentifier, TData> : ITemp<TEncoded, TEncodedIdentifier, TData>
    {
        protected List<TempedView<TEncodedIdentifier, TData>> TempedViews;
        protected CellService CellService;
        internal Dictionary<TEncoded, TData> GetAllResourceData => Data;
        public Dictionary<TEncoded, TData> Data { get; set; }
        public Dictionary<TEncodedIdentifier, TEncoded> EncodeByIdentifier { get; set; }
        public Dictionary<TEncodedIdentifier, TData> DataByIdentifier { get; set; }
        public Dictionary<TEncodedIdentifier, int> ViewIndex { get; set; }
        internal IIdentifier<TEncodedIdentifier, TEncoded> Identifier;
        
        protected internal Temp(IIdentifier<TEncodedIdentifier, TEncoded> identifier)
        {
            Identifier = identifier;
            Data = new Dictionary<TEncoded, TData>();
            EncodeByIdentifier = new Dictionary<TEncodedIdentifier, TEncoded>();
            DataByIdentifier = new Dictionary<TEncodedIdentifier, TData>();
            ViewIndex = new Dictionary<TEncodedIdentifier, int>();
            TempedViews = new List<TempedView<TEncodedIdentifier,TData>>();
        }
        protected virtual TEncodedIdentifier GetIdentifierByEncoded(TEncoded encoded)
        {
            return Identifier.GetEncodedIdentifier(encoded);
        }

        protected virtual TData SummedAmounts(TData a, TData b)
        {
            return a;
        }
        protected virtual TData SubtractionAmounts(TData a, TData b)
        {
            return a;
        }
        internal void SubtractionAmountData(TEncodedIdentifier identifier, TData value)
        {
            Data[EncodeByIdentifier[identifier]] = SubtractionAmounts(Data[EncodeByIdentifier[identifier]], value);
        }
        internal void SummedAmountData(TEncodedIdentifier identifier, TData value)
        {
            Data[EncodeByIdentifier[identifier]] = SummedAmounts(Data[EncodeByIdentifier[identifier]], value);
        }
        internal TData GetAmount(TEncodedIdentifier identifier)
        {
            return DataByIdentifier[identifier];
        }
        internal void SetAmount(TEncodedIdentifier identifier, TData value)
        {
            Data[EncodeByIdentifier[identifier]] = value;
        }
        /// <summary>
        /// Injection dependency
        /// </summary>
        internal virtual void Injection()
        {
            CellService = SessionStateManager.Instance.Container.Resolve<CellService>();
        }

        internal bool Contains(TEncodedIdentifier identifier)
        {
            if (EncodeByIdentifier.ContainsKey(identifier)) return true;
            return false;
        }
        internal bool Contains(TEncoded encoded)
        {
            if (EncodeByIdentifier.ContainsKey(GetIdentifierByEncoded(encoded))) return true;
            return false;
        }
        public void Temped(TEncoded encode, TData amount)
        {
            var identifier = GetIdentifierByEncoded(encode);
            if (EncodeByIdentifier.ContainsKey(identifier))
            {
                EncodeByIdentifier[identifier] = encode;
                DataByIdentifier[identifier] = amount;
                Data[encode] = amount;

#if UNITY_EDITOR
                ViewIndex[identifier] = TempedViews.Count;
                TempedViews[ViewIndex[identifier]-1].value = amount;
                TempedViews[ViewIndex[identifier]-1].title = identifier;
#endif
            }
            else
            {
                Initialization(encode, amount);
            }
        }
        public void Initialization(TEncoded encoded, TData amount)
        {
            Debugger.Logger($"{encoded.ToString()} with amount {amount.ToString()} add to temp", ContextDebug.Application,Process.Load);
            var identifier = GetIdentifierByEncoded(encoded);
            Data.Add(encoded, amount);
            EncodeByIdentifier.Add(identifier,encoded);
            DataByIdentifier.Add(identifier,amount);
            ViewIndex.Add(identifier,TempedViews.Count);
            TempedViews.Add(new TempedView<TEncodedIdentifier,TData>(identifier, amount));
        }
    }
}