using Game.Core;
using UnityEngine;

namespace Game.Services.Storage.MapCellsRepository
{
    [System.Serializable]
    public class MapCellCoded
    {
        public string encryptedContainerName;
        public string encryptedContainerStateName;
        public string encryptedCellPosition;
        public string encryptedCellCord;
        public string encryptedCellScale;
        public MapCellCoded(string name, string state, string position,string cord, string scale)
        {
            encryptedContainerName = name;
            encryptedContainerStateName = state;
            encryptedCellPosition = position;
            encryptedCellScale = scale;
            encryptedCellCord = cord;
        }
        public override string ToString()
        {
            return $"{encryptedContainerName}_{encryptedContainerStateName}_{encryptedCellPosition}_{encryptedCellCord}_{encryptedCellScale}";
        }
    }
}