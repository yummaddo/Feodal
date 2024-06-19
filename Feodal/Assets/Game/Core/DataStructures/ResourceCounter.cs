namespace Game.Core.DataStructures
{
    
#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;
    using Game.Core.DataStructures;

    [CustomPropertyDrawer(typeof(ResourceCounter))]
    public class ResourceCounterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Calculate rects
            Rect resourceRect = new Rect(position.x, position.y, position.width * 0.6f, position.height);
            Rect valueRect = new Rect(position.x + position.width * 0.65f, position.y, position.width * 0.35f, position.height);

            // Get properties
            SerializedProperty resourceProperty = property.FindPropertyRelative("resource");
            SerializedProperty valueProperty = property.FindPropertyRelative("value");

            // Draw fields
            EditorGUI.PropertyField(resourceRect, resourceProperty, GUIContent.none);
            EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
#endif
    [System.Serializable]
    public class ResourceCounter
    {
        public Resource resource;
        public int value;

        public ResourceCounter(Resource resource, int value)
        {
            this.resource = resource;
            this.value = value;
        }
    }
}