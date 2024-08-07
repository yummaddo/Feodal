﻿using System;
using System.Collections.Generic;
using System.IO;
using Game.Utility;
using UnityEngine;

namespace Game.RepositoryEngine.Abstraction
{
    [System.Serializable]
    public abstract class Repository<TCoded,TEncoded,TData,TEncodedIdentifier,TTemp> : IDisposable
    
        where TTemp : Temp<TEncoded, TEncodedIdentifier, TData>
    {
        [SerializeField] internal TTemp temp;
        protected static string SaveFileName { get; set; }
        /// <summary>
        /// Parameter that will be contained in repository data file
        /// </summary>
        private List<EncryptData> _data;
        protected Dictionary<TEncoded, TData> ResourceData { get; set; }
        protected string DataJson { get; set; }
        protected int Count { get; set; }
        protected List<TEncoded> Encodes { get; set; }
        protected string SaveFileResourcePath { get; set; }
        protected abstract object PublicAesLock { get; set; }
        protected  abstract object  PrivateAesLock { get; set; }
        
        public void Dispose()
        {
            ResourceData?.Clear();
            _data?.Clear();
            Encodes?.Clear();
        }
        ~Repository()
        {
            Dispose();
        }

        protected abstract void UpdateName();
        internal virtual void Initialization( IIdentifier<TEncodedIdentifier,TEncoded> convert, List<TEncoded> encodes = null )
        {
            UpdateName();
            Debugger.Logger($"Repository {this.GetType().Name} Init Temp {typeof(TTemp).Name}", ContextDebug.Initialization, Process.Update);
            InitTemp(convert);
            if (encodes != null) InitHimselfEncodes(encodes);
            else Encodes = new List<TEncoded>();

        }
        #region Abstraction
        /// <summary>
        /// Create  Decrypted TData D from encrypted Data
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public abstract TData ParseDecryptedValue(string[] decryptString);
        /// <summary>
        /// Create Encrypted Data string[]  from free TData
        /// </summary>
        /// <param name="encryptData"></param>
        /// <returns></returns>
        public abstract string[] CreateEncryptValue(TData encryptData);
        /// <summary>
        /// Memory allocation for data value in free Repository element 
        /// </summary>
        /// <returns></returns>
        protected abstract TData GetNewRepositoryAmount();
        /// <summary>
        /// Coding the [resource] with RSA code invariants
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        protected abstract TCoded Encrypt(TEncoded resource);
        /// <summary>
        /// De-Coding the [codedResource] with RSA code in TEncoded repository data container type
        /// </summary>
        /// <param name="codedResource"></param>
        /// <returns></returns>
        protected abstract TEncoded Decrypt(TCoded codedResource);
        protected abstract void InitTemp(
            IIdentifier<TEncodedIdentifier,TEncoded> converter
            );
        /// <summary>
        /// Initialization Spase fore temp than would be contain the Repository load data 
        /// </summary>
        /// <param name="resourceEncode"></param>
        protected void InitHimselfEncodes(List<TEncoded> resourceEncode)
        {
            Encodes = resourceEncode;
            foreach (var encoded in resourceEncode)
            {
                temp.Initialization(encoded, GetNewRepositoryAmount());
            }
        }
        #endregion
        #region Internal Methods
                internal void SaveResourceData()
        {
            try
            {
                var resourceData = temp.GetAllResourceData;
                RepositoryInst(resourceData);
                if (resourceData.Keys.Count != 0)
                {
                    var json =  GetJsonData();                
                    Data dataEncrypt = new Data(_data);
                    WriteDataToJson(dataEncrypt);
                }
            }
            catch (Exception e)
            {
                
                Debugger.Logger(e.Message, Process.TrashHold);
            }

        }
        
        internal void LoadResourceData()
        {
            try
            {
                Debugger.Logger($"Load Resource Data {GetType()}", Process.Process);
                if (File.Exists(DataPath()))
                {
                    Debugger.Logger(DataPath(), ContextDebug.Application, Process.Info);
                    var json = ReadDataFromJson();
                    Debugger.Logger($"Read json {json.Length} literals", ContextDebug.Application, Process.Load);
                    RepositoryInst(json);
                }
                else
                {
                    Debugger.Logger("File Doesnt exist: path="+ DataPath(), ContextDebug.Application, Process.Info);
                    Data data = new Data(new List<EncryptData>());
                    var voidJson = JsonUtility.ToJson(data);
                    WriteDataToJson(data);
                    DataJson = voidJson;
                    ResourceData = new Dictionary<TEncoded, TData>();
                    lock (PublicAesLock)
                    {
                        _data = new List<EncryptData>();
                    }
                }
                UpdateFromRepository();
            }
            catch (Exception e)
            {
                
                Debugger.Logger(e.Message, Process.TrashHold);
            }
        
        }
      public static void WriteDataToJson(Data savedData) {
              string dataString;
              string jsonFilePath = DataPath();
              CheckFileExistance(jsonFilePath);
              dataString = JsonUtility.ToJson(savedData);
              File.WriteAllText(jsonFilePath, dataString);
      }
      public  string ReadDataFromJson() {
              string dataString;
              string jsonFilePath = DataPath();
              CheckFileExistance(jsonFilePath, true);
              dataString = File.ReadAllText(jsonFilePath);
              return dataString;
      }

        static string DataPath() {
// #if UNITY_EDITOR
//             return Application.streamingAssetsPath + $"/{SaveFileName}";
// #endif
              if (Directory.Exists(Application.persistentDataPath))
              {
                  return Application.persistentDataPath + $"/{SaveFileName}";
              }
              return Path.Combine(Application.streamingAssetsPath + $"/{SaveFileName}");
      }

        static void CheckFileExistance(string filePath, bool isReading = false) {
              if (!File.Exists(filePath)){
                  File.Create(filePath).Close();
                  if (isReading) {
                    Data data = new Data(new List<EncryptData>());
                    var voidJson = JsonUtility.ToJson(data);
                    File.WriteAllText(filePath, voidJson);
                  }
              }
      }
        /// <summary>
        ///  get data form temp object
        /// </summary>
        /// <param name="resourceData"></param>
        protected bool RepositoryInst(Dictionary<TEncoded, TData> resourceData)  //
        {
            lock (PublicAesLock)
            {
                try
                {
                    ResourceData = resourceData;
                    _data = new List<EncryptData>();
                    
                    foreach (var kvp in resourceData)
                    {
                        var resCoded = Encrypt(kvp.Key);
                        var dataElement = new EncryptData(resCoded, CreateEncryptValue(kvp.Value));
                        _data.Add(dataElement);
                    }
                    
                    DataJson = GetJsonData();
                    return true;
                }
                catch (Exception e)
                {
                    Debugger.Logger(e.Message, $"Repository Inst {this.GetType()}", ContextDebug.Session, Process.TrashHold);
                    return false;
                }
            }
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
            lock (PublicAesLock)
            {
                var storageData = JsonUtility.FromJson<Data>(json);
                if (storageData == null)
                {
                    return null;
                }
                var storage = storageData.data;
                ResourceData = new Dictionary<TEncoded, TData>();
                if (storage == null)
                {
                    return ResourceData;
                }
                foreach (var resourceEncrypt in storage)
                {
                    var decryptedResource = Decrypt(resourceEncrypt.resource);
                    var valueCoded = ParseDecryptedValue(resourceEncrypt.value);
                    ResourceData.Add(decryptedResource, valueCoded);
                }
            }
            return ResourceData;
        }

        internal static void Reset(string repositoryName)
        {
            WriteDataToJson(new Data(new List<EncryptData>()));
        }

        /// <summary>
        /// Get coded string by dump of information 
        /// </summary>
        /// <returns></returns>
        protected string GetJsonData()
        {
            lock (PublicAesLock)
            {

                Data dataEncrypt = new Data(_data);
                return JsonUtility.ToJson(dataEncrypt);
            }
        }
        /// <summary>
        /// Update the Temp element information
        /// </summary>
        /// <param name="identifiers"></param>
        private void Update(List<TEncoded> identifiers)
        {
            foreach (var identifier in identifiers)
            {
                if (!temp.Contains(identifier))
                {
                    temp.Initialization(identifier, GetNewRepositoryAmount());
                }
            }
        }
        private void UpdateFromRepository()
        {
            try
            {
                foreach (var key in ResourceData)
                    temp.Temped(key.Key, key.Value);
            }
            catch (Exception e)
            {
                Debugger.Logger(e.Message, Process.TrashHold);
            }
        }
        #endregion
        #region Encrypt
        // XOR  Cn = Mn xor Kn
        protected string EncryptString(string input) => Cipher(input);
        protected string DecryptString(string encryptedInput) => Cipher(encryptedInput);
        private string Cipher(string text)
        {
            return text;
            // var secretKey = ApplicationSetting.Key;
            // var currentKey = GetRepeatKey(secretKey, text.Length);
            // var res = string.Empty;
            // for (var i = 0; i < text.Length; i++)
            // {
            //     res += ((char)(text[i] ^ currentKey[i])).ToString();
            // } 
            // return res;
        }
        private string GetRepeatKey(string pass, int numeric)
        {
            var r = pass;
            while (r.Length < numeric) { r += r; }
            return r.Substring(0, numeric);
        }

        #endregion
        [System.Serializable]
        public class EncryptData
        {
            public TCoded resource;
            public string[] value;

            public EncryptData(TCoded resource, string[] value)
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