﻿using System;
using System.Collections.Generic;
using Game.Core.DataStructures.Technologies;
using Game.Services.CellControlling;
using Game.Services.Proxies;
using Game.Services.Proxies.CallBackTrade;
using Game.Services.Proxies.Providers.TradeProviders;
using Game.Services.Storage.Abstraction;
using Game.Services.Storage.ResourcesRepository;
using UnityEngine;

namespace Game.Services.Storage.TechnologyRepositories
{
    [System.Serializable]
    public class TechnologyRepository : Repository<TechnologyCoded, TechnologyEncoded, bool, string, TechnologyTemp>
    {
        // ResourceRepository Custom Data Spase
        private CellService _cellService;
        // Base
        [field: SerializeField] protected override string SaveFileName { get; set; } = "TechnologyRepository.json";
        protected override object PublicAesLock { get; set; } = new object();
        protected override object PrivateAesLock { get; set; } = new object();
        
        internal void Injection(CellService service, List<Technology> resources)
        {
            _cellService = service;
            Proxy.Connect<TechnologyTradeProvider, TechnologyTradeCallBack,TechnologyTradeCallBack>(OnTradeTechnology);
            List<TechnologyEncoded> data = new List<TechnologyEncoded>();
            foreach (var resource in resources)
            {
                data.Add(new TechnologyEncoded(resource));
            }
            Encodes = data;
        }

        public void OnTradeTechnology(Port port, TechnologyTradeCallBack callBack)
        {
            var newTech = callBack.Trade.@into.Data.Title;
            var encodedTech = temp.EncodeByIdentifier[newTech];
            temp.SetAmount(encodedTech, newTech, true);
        }

        public override bool ParseDecryptedValue(string[] decryptString)
        {
            try
            {   
                return bool.Parse(DecryptString(decryptString[0]));
            }
            catch (Exception e)
            {
                Debugger.Logger( $"decryptString[0]={DecryptString(decryptString[0])} "+e.Message);
            }
            return false;
        }
        
        public override string[] CreateEncryptValue(bool encryptData)
        {
            return new string[] { EncryptString(encryptData.ToString()) };
        }
        
        protected override bool GetNewRepositoryAmount()
        {
            return false;
        }
        
        protected override TechnologyCoded Encrypt(TechnologyEncoded resource)
        {
            return new TechnologyCoded(EncryptString(resource.title));
        }
        
        protected override TechnologyEncoded Decrypt(TechnologyCoded codedResource)
        {
            return new TechnologyEncoded(DecryptString(codedResource.encodedTitle));
        }

        protected override void InitTemp(IIdentifier<string, TechnologyEncoded> converter)
        {
            temp = new TechnologyTemp(converter);
        }
    }
}