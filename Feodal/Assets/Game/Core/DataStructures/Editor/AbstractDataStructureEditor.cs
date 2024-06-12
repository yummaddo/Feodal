#if UNITY_EDITOR
namespace Game.Core.DataStructures.Editor
{
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    public abstract class AbstractDataStructureEditor<T, TTemplate> : Editor 
        
        where T : AbstractDataStructure<TTemplate>
    
    {
        private MethodInfo _method = null;

        private void OnEnable()
        {
            if (_method == null)
            {        
                _method = typeof(T).GetMethod(
                "ChangeStorageNaming",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            }
        }
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var dataStructure = (T)target;
            if (GUILayout.Button("Validate"))
            {
                if (_method != null)
                {
                    _method.Invoke(dataStructure, new object[] { dataStructure.Data });
                }
                EditorUtility.SetDirty(dataStructure);
            }
        }
    }
}
#endif