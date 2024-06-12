using UnityEngine;

namespace Game.Core.DataStructures
{
    public abstract class AbstractDataStructure<TTemplate> : ScriptableObject
    {
        public TTemplate Data => CompareTemplate();
        protected abstract TTemplate CompareTemplate();

        protected virtual bool IsStorageChanged()
        {
            if (DataNamePattern == name)
            {
                return false;
            }

            return true;
        }

        protected abstract string DataNamePattern { get; }

        protected virtual void ChangeStorageNaming(TTemplate type)
        {
            if (this.IsStorageChanged()) { RenameAsset(); }
        }
        private void RenameAsset()
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
            UnityEditor.AssetDatabase.RenameAsset(assetPath, DataNamePattern);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        
    }
}