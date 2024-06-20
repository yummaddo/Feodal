using System;
using System.Collections.Generic;
using Game.Core;
using Game.Core.Abstraction;
using Game.Meta;
using Game.Services.CellControlling;
using Game.Services.Storage.Abstraction;
using Game.Services.Storage.ResourcesRepository;
using HexEngine;
using UnityEngine;

namespace Game.Services.Storage.MapCellsRepository
{
    [System.Serializable]
    public class MapCellRepository : Repository<MapCellCoded, MapCellEncoded, string, HexCoords, MapCellTemp>
    {
        // MapCellRepository Custom Data Spase
        [SerializeField] private CellMap resourceListMap;
        // Base
        [field: SerializeField] protected override string SaveFileName { get; set; } = "MapRepository.json";
        protected override object PublicAesLock { get; set; } = new object();
        protected override object PrivateAesLock { get; set; } = new object();
        
        public override string ParseDecryptedValue(string[] decryptString)
        {
            return DecryptString(decryptString[0]);
        }
        
        public override string[] CreateEncryptValue(string encryptData)
        {
            return new string[] { EncryptString(encryptData) };
        }
        
        protected override string GetNewRepositoryAmount()
        {
            return "Cell";
        }
        
        protected override MapCellCoded Encrypt(MapCellEncoded resource)
        {
            var position = new string[]
            {
                EncryptString(resource.cellPosition.x.ToString()), 
                EncryptString(resource.cellPosition.y.ToString()),
                EncryptString(resource.cellPosition.z.ToString())
            };
            var cord = new string[]
            {
                EncryptString(resource.cellCoord.X.ToString()), 
                EncryptString(resource.cellCoord.Y.ToString())
            };
            return new MapCellCoded(
                EncryptString(resource.containerName),
                EncryptString(resource.containerStateName),
                    position,
                    cord,
                EncryptString(resource.cellScale.ToString()));
        }
        
        protected override MapCellEncoded Decrypt(MapCellCoded codedResource)
        {
            var name = DecryptString(codedResource.encryptedContainerName);
            var state = DecryptString(codedResource.encryptedCellScale);
            var vectorPosition = new Vector3(0, 0, 0);
            try
            {
                var x = float.Parse(DecryptString(codedResource.encryptedCellPosition[0]));
                vectorPosition.x = x;
                var y = float.Parse(DecryptString(codedResource.encryptedCellPosition[1]));
                vectorPosition.y= y;
                var z = float.Parse(DecryptString(codedResource.encryptedCellPosition[2]));
                vectorPosition.z = z;
            }
            catch (Exception e)
            {
                Debugger.Logger(e.Message, Process.TrashHold);
            }
            var cordX = int.Parse(DecryptString(codedResource.encryptedCellCord[0]));
            var cordY = int.Parse(DecryptString(codedResource.encryptedCellCord[1]));
            var vectorCord = new HexCoords(cordX, cordY);
            var scale = float.Parse(DecryptString(codedResource.encryptedCellPosition[2]));
            return new MapCellEncoded(name, state, vectorPosition, vectorCord, scale);
        }
        protected override void InitTemp(IIdentifier<HexCoords,MapCellEncoded> convert)
        {
            temp = new MapCellTemp(convert);
        }
    }

}