namespace Game.Core.DataStructures.Editor
{
#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;
    [CustomPropertyDrawer(typeof(ResourceCounter))]
    public class ResourceCounterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Calculate rects
            Rect resourceRect = new Rect(position.x, position.y, position.width * 0.8f, position.height);
            Rect valueRect = new Rect(position.x + position.width * 0.82f, position.y, position.width * 0.17f, position.height);

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
}