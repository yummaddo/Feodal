using Game.Core.DataStructures.Technologies;
using UnityEngine;

namespace Game.Core.DataStructures
{
    public abstract class AbstractDataStructure<TTemplate> : ScriptableObject
    {
        public virtual TTemplate Data => CompareTemplate();
        protected abstract TTemplate CompareTemplate();

        protected virtual bool IsStorageChanged()
        {
            // if (DataNamePattern == name)
            // {
            //     return false;
            // }

            return true;
        }

        public override string ToString()
        {
            return DataNamePattern;
        }

        internal abstract string DataNamePattern { get; }
        internal void ChangeStorageNaming(TTemplate type)
        {
            RenameAsset();
        }
        [ContextMenu("RenameAsset")]
        public virtual void RenameAsset()
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
            UnityEditor.AssetDatabase.RenameAsset(assetPath, DataNamePattern);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        
    }
}