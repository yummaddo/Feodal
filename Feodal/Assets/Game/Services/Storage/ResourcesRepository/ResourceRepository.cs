using System;
using System.Collections.Generic;
using Codice.CM.Common;
using Game.Core.DataStructures;
using Game.Core.DataStructures.Conditions.TradesConditions;
using Game.Core.DataStructures.Technologies.Abstraction;
using Game.Core.Typing;
using Game.Services.CellControlling;
using Game.Services.Storage.Abstraction;
using Game.Services.Storage.TechnologyRepositories;
using UnityEngine;

namespace Game.Services.Storage.ResourcesRepository
{
    [System.Serializable]
    public class ResourceRepository : Repository<ResourceCoded, ResourceEncoded, long, string, ResourceTemp>
    {
        // ResourceRepository Custom Data Spase
        private CellService _cellService;
        // Base
        [field: SerializeField] protected override string SaveFileName { get; set; } = "ResourceRepository.json";
        protected override object PublicAesLock { get; set; } = new object();
        protected override object PrivateAesLock { get; set; } = new object();

        internal void Injection(CellService service, List<Resource> resources)
        {
            _cellService = service;
            List<ResourceEncoded> data = new List<ResourceEncoded>();
            foreach (var resource in resources)
            {
                data.Add(new ResourceEncoded(resource.Data));
            }
            Encodes = data;
        }
        
        public override long ParseDecryptedValue(string[] decryptString)
        {
            try
            {
                return long.Parse( DecryptString(decryptString[0]));
            }
            catch (Exception e)
            {
                Debugger.Logger(DecryptString(decryptString[0]));
                Debugger.Logger(e.Message, Process.TrashHold);
                return GetNewRepositoryAmount();
            }
        }
        
        public override string[] CreateEncryptValue(long encryptData)
        {
            return new string[] { EncryptString(encryptData.ToString()) };
        }
        
        protected override long GetNewRepositoryAmount()
        {
            return 0;
        }
        
        protected override ResourceCoded Encrypt(ResourceEncoded resource)
        {
            return new ResourceCoded(EncryptString(resource.Title));
        }
        protected override ResourceEncoded Decrypt(ResourceCoded codedResource)
        {
            return new ResourceEncoded(DecryptString(codedResource.encryptedTitle));
        }
        protected override void InitTemp(IIdentifier<string, ResourceEncoded> converter)
        {
            temp = new ResourceTemp(converter);
        }
    }
}