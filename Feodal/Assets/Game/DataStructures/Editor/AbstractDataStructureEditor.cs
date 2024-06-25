using System.Reflection;
using Game.DataStructures.Abstraction;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Game.DataStructures.Editor
{
    public abstract class AbstractDataStructureEditor<T, TTemplate> : UnityEditor.Editor 
        
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