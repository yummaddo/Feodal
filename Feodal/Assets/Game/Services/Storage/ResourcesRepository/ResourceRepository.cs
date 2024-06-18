using System;
using System.Collections.Generic;
using Game.Core.DataStructures;
using Game.Core.Typing;
using UnityEngine;

namespace Game.Services.Storage.ResourcesRepository
{
    [System.Serializable]
    public class ResourceRepository : Repository<ResourceCoded, ResourceEncoded, long, string>
    {
        // ResourceRepository Custom Data Spase
        
        [SerializeField] private List<Resource> resourceList = new List<Resource>();
        [field: SerializeField] protected override string SaveFileName { get; set; } = "ResourceRepository.json";
        // Base
        protected override string PrivateRsa { get; set; } = 
            "<RSAKeyValue>" +
            "<Modulus>g26LHn3/QpILfo5NIlEqia26nhQc5jXRg+3nhVGGUnzO5C6wu0a1t5qa09vOLf3oogRQ9XWqi51n3zGkxbyZMMNe3oWqppAlrQ4ljFmsGIZhibgwvjSQx2cby4GwgnCyfbfPrxlXzEQG7X6TwSz30W6FcMK+R0Q+kNl4TpoQKas=</Modulus>" +
            "<Exponent>AQAB</Exponent>" +
            "<P>lZuz5B8y2MF1jyHoyv7XbpQU7qq4z3o9YZs+VMs9KixLbec4qma/EIQB1nRhx/yOwcmQMZkwZHeWhhQUcvHRvw==</P>" +
            "<Q>4OXPygdjlrBs7+pMkfvPrdpyhQVKztqOACqwXZtb49+T4YYeMD6U5bELWqlh5rp2SVce6C4LFAD9QtqS7/lLFQ==</Q>" +
            "<DP>KBTc7yMEplm+Oyiki64W3YNC0GZguQVQ6JOE8Ya5zlCrrLgj0FNSoQQc0e3APKoHPRfZT9BwzJnVc/pxOBJdVQ==</DP>" +
            "<DQ>IeI+BkBKvt9h6luwoaYAyj7JVkVP2g6PqnLEE7Zeh9ydmTLtxXMIV/rurQAsIekh/TbFD1IwlRj5D2ODE+jJaQ==</DQ>" +
            "<InverseQ>hwOa86EXN4Fxi8k8jbajXL6jkO3IAqMZ+MoHV/bvJt8aJkXUrawMOt+agKZ/s+wUBZH9yIjdpJj9uqtvxIAwSw==</InverseQ>" +
            "<D>Vqfp3d7hEDlOjtCwFrJBQ6lh45wLOdW+ICgZgBMyZuyXVGdcav3CDh5Heuvv0u8YFMqXvl8oZQkRVV1b8Gva7A7X229BSYjmQeV7pZjSPZ7HdHVKEru/LPkvlXiumMxMYyEbxmcAMMXOXP91jf9SJfBanD0VNZfTIHlo6PE44nE=</D>" +
            "</RSAKeyValue>";
        protected override string PublicRsa { get; set; } =
            "<RSAKeyValue>" +
            "<Modulus>g26LHn3/QpILfo5NIlEqia26nhQc5jXRg+3nhVGGUnzO5C6wu0a1t5qa09vOLf3oogRQ9XWqi51n3zGkxbyZMMNe3oWqppAlrQ4ljFmsGIZhibgwvjSQx2cby4GwgnCyfbfPrxlXzEQG7X6TwSz30W6FcMK+R0Q+kNl4TpoQKas=</Modulus>" +
            "<Exponent>AQAB</Exponent>" +
            "</RSAKeyValue>";
        protected override object PublicRsaLock { get; set; } = new object();
        protected override object PrivateRsaLock { get; set; } = new object();
        public override long ParseDecryptedValue(string decryptString)
        {
            return long.Parse(decryptString);
        }
        protected override long GetNewRepositoryAmount()
        {
            return 0;
        }
        protected override ResourceCoded Encrypt(ResourceEncoded resource)
        {
            return new ResourceCoded(
                EncryptString(resource.Title),
                EncryptString(resource.Type.ToString()), 
                EncryptString(resource.Rare.ToString()), 
                EncryptString(resource.Quantity.ToString()));
        }
        protected override ResourceEncoded Decrypt(ResourceCoded codedResource)
        {
            return new ResourceEncoded(
                DecryptString(codedResource.encryptedTitle),
                (ResourceType)Enum.Parse(typeof(ResourceType), DecryptString(codedResource.encryptedType)),
                (ResourceRareType)Enum.Parse(typeof(ResourceRareType), DecryptString(codedResource.encryptedRare)),
                int.Parse(DecryptString(codedResource.encryptedQuantity))
            );
        }
        protected override List<ResourceEncoded> InitHimselfEncodes()
        {
            var res = new List<ResourceEncoded>();
            foreach (var resource in resourceList)
            {
                res.Add(new ResourceEncoded(resource.Data));
            }
            return res;
        }
        protected override void InitTemp()
        {
            Temp = new ResourceTemp();
        }
    }
}