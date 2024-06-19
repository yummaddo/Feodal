using System.Collections.Generic;
using HexEngine;
using UnityEngine;

namespace Game.Services.Storage.MapCellsRepository
{
    public class MapCellRepository : Repository<MapCellCoded, MapCellEncoded, HexCoords, GameObject>
    {
        [field: SerializeField] protected override string SaveFileName { get; set; } = "MapRepository.json";
        protected override void InitTemp()
        {
        }
        protected override object PublicRsaLock { get; set; }
        protected override object PrivateRsaLock { get; set; }
        protected override string PublicRsa { get; set; }
        protected override string PrivateRsa { get; set; }
        public override HexCoords ParseDecryptedValue(string decryptString)
        {
            throw new System.NotImplementedException();
        }
        protected override HexCoords GetNewRepositoryAmount()
        {
            return new HexCoords(0, 0);
        }
        protected override MapCellCoded Encrypt(MapCellEncoded resource)
        {
            throw new System.NotImplementedException();
        }
        protected override MapCellEncoded Decrypt(MapCellCoded codedResource)
        {
            throw new System.NotImplementedException();
        }
        protected override List<MapCellEncoded> InitHimselfEncodes()
        {
            throw new System.NotImplementedException();
        }
    }
}