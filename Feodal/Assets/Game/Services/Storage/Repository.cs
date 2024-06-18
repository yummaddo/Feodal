using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Game.Core.Abstraction;
using Game.Services.Storage.ResourcesRepository;
using UnityEngine;

namespace Game.Services.Storage
{
    [System.Serializable]
    public abstract class Repository<TCoded,TEncoded,TData,TEncodedIdentifier>
    {
        /// <summary>
        /// Repository File Name
        /// </summary>
        protected abstract string SaveFileName { get; set; }
        /// <summary>
        /// Parameter that will be contained in repository data file
        /// </summary>
        [SerializeField] private List<EncryptData> data;
        protected Dictionary<TEncoded, TData> ResourceData { get; set; }
        protected string DataJson { get; set; }
        protected int Count { get; set; }
        protected List<TEncoded> Encodes { get; set; }
        internal virtual void Initialization()
        {
            Encodes = InitHimselfEncodes();
            SaveFileResourcePath = InitHimselfDataFilePath();
            InitTemp();
        }
        protected string SaveFileResourcePath { get; set; }
        protected abstract void InitTemp();
        internal Temp<TEncoded, TEncodedIdentifier, TData> Temp { get; set; }
        internal void SaveResourceData()
        {
            var resourceData = Temp.GetAllResourceData;
            RepositoryInst(resourceData);
            var json =  GetJsonData(); 
            File.WriteAllText(SaveFileResourcePath, json);
        }
        internal void LoadResourceData()
        {
            if (File.Exists(SaveFileResourcePath))
            {
                Debug.Log(SaveFileResourcePath);
                var json = File.ReadAllText(SaveFileResourcePath);
                RepositoryInst(json);
                Update(Encodes);
            }
            else
            {
                File.Create(SaveFileResourcePath);
                Update(Encodes);
            }
        }
        #region Abstraction
        protected abstract object PublicRsaLock { get; set; }
        protected  abstract object  PrivateRsaLock { get; set; }
        protected abstract string PublicRsa { get; set; }
        protected  abstract string  PrivateRsa { get; set; }
        public abstract TData ParseDecryptedValue(string decryptString);
        protected abstract TData GetNewRepositoryAmount();
        protected abstract TCoded Encrypt(TEncoded resource);
        protected abstract TEncoded Decrypt(TCoded codedResource);
        protected abstract List<TEncoded> InitHimselfEncodes();
        #endregion
        
        internal virtual string InitHimselfDataFilePath()
        {
            return Application.dataPath + $"/Resource/Database/{SaveFileName}";
        }
        
        #region Internal Methods
        /// <summary>
        ///  get data form temp object
        /// </summary>
        /// <param name="resourceData"></param>
        protected void RepositoryInst(Dictionary<TEncoded, TData> resourceData)  //
        {
            ResourceData = resourceData;
            data = new List<EncryptData>();
            foreach (var kvp in resourceData)
            {
                var resCoded = Encrypt(kvp.Key);
                var valueCoded = EncryptString(kvp.Value.ToString());
                var dataElement = new EncryptData(resCoded, valueCoded);
                data.Add(dataElement);
            }
            Debug.Log(data.Count);
            DataJson = GetJsonData();
        }
        /// <summary>
        /// get data form external storage
        /// </summary>
        /// <param name="jsonData"></param>
        protected void RepositoryInst(string jsonData) // 
        {
            DataJson = jsonData;
            GetData(jsonData);
        }
        /// <summary>
        /// Get dump of decoded information
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        protected Dictionary<TEncoded, TData> GetData(string json)
        {
            var storageData = JsonUtility.FromJson<Data>(json);
            var storage = storageData.data;
            Debug.Log(storage);
            ResourceData = new Dictionary<TEncoded, TData>();
            if (storage == null)
            {
                return ResourceData;
            }
            foreach (var resourceEncrypt in storage)
            {
                var decryptedResource = Decrypt(resourceEncrypt.resource);
                // var decryptedValue = long.Parse(DecryptString(resourceEncrypt.value));
                var decryptedValue = ParseDecryptedValue(DecryptString(resourceEncrypt.value));
                ResourceData.Add(decryptedResource, decryptedValue);
            }
            return ResourceData;
        }
        /// <summary>
        /// Get coded string by dump of information 
        /// </summary>
        /// <returns></returns>
        protected string GetJsonData()
        {
            Data dataEncrypt = new Data(data);
            Debugger.Logger(JsonUtility.ToJson(dataEncrypt));
            return JsonUtility.ToJson(dataEncrypt);
        }
        /// <summary>
        /// Update the Temp element information
        /// </summary>
        /// <param name="identifiers"></param>
        private void Update(List<TEncoded> identifiers)
        {
            foreach (var identifier in identifiers)
            {
                if (!Temp.Contains(identifier))
                {
                    Temp.Initialization(identifier, GetNewRepositoryAmount());
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected string EncryptString(string input)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(input);
            lock (PublicRsaLock)
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(PublicRsa);
                    byte[] encryptedBytes = rsa.Encrypt(dataBytes, true);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedInput"></param>
        /// <returns></returns>
        protected string DecryptString(string encryptedInput)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedInput);
            lock (PrivateRsaLock)
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(PrivateRsa);
                    byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, true);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        #endregion

        [System.Serializable]
        public class EncryptData
        {
            public TCoded resource;
            public string value;

            public EncryptData(TCoded resource, string value)
            {
                this.resource = resource;
                this.value = value;
            }
        }
        [System.Serializable]
        public class Data
        {
            public List<EncryptData> data;

            public Data(List<EncryptData> data)
            {
                this.data = data;
            }
        }
    }
}