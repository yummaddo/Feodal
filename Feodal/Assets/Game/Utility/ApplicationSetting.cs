using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Game.Utility
{
    [System.Serializable]
    public class ApplicationSetting
    {
        [field: SerializeField] internal string ResourceSaveFileName { get; set; } = "ResourceRepository.json";
        [field: SerializeField] internal string TechnologySaveFileName { get; set; } = "TechnologyRepository.json";
        [field: SerializeField] internal string MapSaveFileName { get; set; } = "MapRepository.json";
        /// <summary>
        /// https://ru.wikipedia.org/wiki/RSA
        /// https://web.archive.org/web/20170123070805/http://www.cs.umd.edu/class/sum2003/cmsc311/Notes/BitOp/xor.html
        /// </summary>
        // private static readonly string key =
        //     "ThisIsTheKeyForDoubleEncryption,InOrderForTheOtherCodeToWork,ItMustBeEncryptedAndAddedToTheEncryptedField";
        private static readonly string keyEncrypt = "UGg4iId9Xzs0Td8KJxC8BNA4QJLHPaMmUUyKpL3Lp1fYzJVwefFzUje4FKJYzB3fF+CFMRFaRccM8FSMYuxreRqgq628UyIkhILuQcNMti8d2GuiPTxqox2XY2Z0O9CbNC6EYhtGEJ/urVQULXJ1Zu2SSKu+kGJbmaN8urDDxaw=";
        private static string PrivateRsa =
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
        private static string PublicRsa  =
            "<RSAKeyValue>" +
            "<Modulus>g26LHn3/QpILfo5NIlEqia26nhQc5jXRg+3nhVGGUnzO5C6wu0a1t5qa09vOLf3oogRQ9XWqi51n3zGkxbyZMMNe3oWqppAlrQ4ljFmsGIZhibgwvjSQx2cby4GwgnCyfbfPrxlXzEQG7X6TwSz30W6FcMK+R0Q+kNl4TpoQKas=</Modulus>" +
            "<Exponent>AQAB</Exponent>" +
            "</RSAKeyValue>";
        
        internal static string RepositoryLocalPath { get; private set; } = "/Resource/Database/";
        internal static string Key => DecryptXorKey(keyEncrypt);
        private static string GenerateRandomKey(int size)
        {
            byte[] keyBytes = new byte[size];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(keyBytes);
            }
            return Convert.ToBase64String(keyBytes);
        }
        private static string EncryptXorKey(string xorKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(ApplicationSetting.PublicRsa);
                byte[] xorKeyBytes = Encoding.UTF8.GetBytes(xorKey);
                byte[] encryptedXorKeyBytes = rsa.Encrypt(xorKeyBytes, false);
                return Convert.ToBase64String(encryptedXorKeyBytes);
            }
        }
        private static string DecryptXorKey(string encryptedXorKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(ApplicationSetting.PrivateRsa);
                byte[] encryptedXorKeyBytes = Convert.FromBase64String(encryptedXorKey);
                byte[] xorKeyBytes = rsa.Decrypt(encryptedXorKeyBytes, false);
                return Encoding.UTF8.GetString(xorKeyBytes);
            }
        }
    }
}