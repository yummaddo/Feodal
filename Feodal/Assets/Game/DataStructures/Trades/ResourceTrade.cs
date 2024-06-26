using System.Collections.Generic;
using Game.DataStructures.Abstraction;
using Game.DataStructures.Technologies;
using Game.DataStructures.Trades.Abstraction;
using Game.DataStructures.Trades.Map;
using Game.Services.StorageServices.Microservice;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Game.DataStructures.Trades
{
    [System.Serializable]
    public class ResourceTrade: AbstractTrade<Resource,ResourceTrade>
    {
        internal ResourceTradeMap Map;
        [SerializeField] internal List<Technology> technologyCondition = new List<Technology>();
        [SerializeField] internal List<ResourceCounter> resourceAmountCondition = new List<ResourceCounter>();
        [SerializeField] internal Resource @into;
        [SerializeField] internal int value;
        public override string TradeName => @into.title;
        internal override void Initialization(TradeMicroservice microservice)
        {
            base.Initialization(microservice);
            Map = new ResourceTradeMap(this);
        }
        public Dictionary<IResource, int> GetBase() => Map.GetAmount(1);
        public override bool IsTradAble() => TradeMicroservice.CanTrade(Map.GetAmount(1));
        public override bool IsTradAble(int amount) => TradeMicroservice.CanTrade(Map.GetAmount(amount));
        public override bool IsTradAbleAll() => TradeMicroservice.CanTrade(Map.GetAmount(1));
        public override void TradeAmount(int amount) { TradeMicroservice.Trade(this, Map.GetAmount(amount), amount); }
        public override void TradeAll() { TradeMicroservice.Trade(this,Map.GetAmount(1),1, true); }
        public override void Trade() { TradeMicroservice.Trade(this,Map.GetAmount(1),1); }


    }
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(ResourceTrade))]
    public class ResourceTradeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lines = 4;
            SerializedProperty technologyCondition = property.FindPropertyRelative("technologyCondition");
            SerializedProperty resourceAmountCondition = property.FindPropertyRelative("resourceAmountCondition");
            lines += Mathf.Max(technologyCondition.arraySize, resourceAmountCondition.arraySize);
            return lines * EditorGUIUtility.singleLineHeight;
        }
        public static void DrawGUI( SerializedProperty property, Rect startPosition, int singleLineHeight)
        {
            if (property == null) return;
            SerializedProperty into = property.FindPropertyRelative("Into");
            SerializedProperty value = property.FindPropertyRelative("Value");
            SerializedProperty technologyCondition = property.FindPropertyRelative("technologyCondition");
            SerializedProperty resourceAmountCondition = property.FindPropertyRelative("resourceAmountCondition");
            
            var resourceAmountConditionsSize= resourceAmountCondition.arraySize;
            var technologyAmountConditionsSize= technologyCondition.arraySize;
            
            Rect resourceListRect = new Rect(
                startPosition.x,
                startPosition.y,
                startPosition.width * 0.5f,
                singleLineHeight);
            Rect resourceListRectDeleteButton = new Rect(
                startPosition.x + startPosition.width * 0.51f,
                startPosition.y,
                startPosition.width * 0.04f,
                singleLineHeight);
            Rect technologyListRect = new Rect(
                startPosition.x + startPosition.width * 0.56f,
                startPosition.y,
                startPosition.width * 0.36f,
                singleLineHeight);
            Rect technologyListRectDeleteButton = new Rect(
                startPosition.x + startPosition.width * 0.92f,
                startPosition.y,
                startPosition.width * 0.04f,
                singleLineHeight);
            var maxHeight = Mathf.Max(technologyAmountConditionsSize, resourceAmountConditionsSize)+2;
            var buttonResourceRectSize = new Rect(resourceListRect) { y = maxHeight*singleLineHeight };
            var buttonTechnologyRectSize = new Rect(technologyListRect) { y = maxHeight*singleLineHeight };
            if (GUI.Button(buttonResourceRectSize, "Add Resource Counter"))
            {
                resourceAmountCondition.arraySize++;
                SerializedProperty newElement = resourceAmountCondition.GetArrayElementAtIndex(resourceAmountCondition.arraySize - 1);
                newElement.FindPropertyRelative("value").intValue = 0;
                newElement.FindPropertyRelative("resource").objectReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            }
            if (GUI.Button(buttonTechnologyRectSize, "Add Technology"))
            {
                technologyCondition.arraySize++;
                SerializedProperty newElement = technologyCondition.GetArrayElementAtIndex(technologyCondition.arraySize - 1);
                newElement.objectReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            }
            // 2 vertical list 
            List<int> indexForDeleteResource = new List<int>();
            for (int i = 0; i < resourceAmountConditionsSize; i++)
            {
                SerializedProperty element = resourceAmountCondition.GetArrayElementAtIndex(i);
                var listRectIteration = singleLineHeight * (i+1);
                var tempRect = new Rect(resourceListRect);
                tempRect.y = listRectIteration;
                EditorGUI.PropertyField(tempRect, element, GUIContent.none);
                var tempButtonRect = new Rect(resourceListRectDeleteButton);
                tempButtonRect.y = listRectIteration;
                if (GUI.Button(tempButtonRect, " - ")) indexForDeleteResource.Add(i);
            }
            
            List<int> indexForDeleteTechnology = new List<int>();
            for (int i = 0; i <technologyAmountConditionsSize; i++)
            {
                SerializedProperty element = technologyCondition.GetArrayElementAtIndex(i);
                var listRectIteration = singleLineHeight * (i+1);
                var tempRect = new Rect(technologyListRect);
                tempRect.y = listRectIteration;
                EditorGUI.PropertyField(tempRect, element, GUIContent.none);
                var tempButtonRect = new Rect(technologyListRectDeleteButton);
                tempButtonRect.y = listRectIteration;
                if (GUI.Button(tempButtonRect, " - ")) indexForDeleteTechnology.Add(i);
            }
            var rectOfInto = new Rect(startPosition);
            rectOfInto.x = startPosition.width * 0.25f;
            rectOfInto.y = (1+maxHeight)*singleLineHeight;
            rectOfInto.width = startPosition.width * 0.6f;
            rectOfInto.height = singleLineHeight;
            var rectOfValue = new Rect(startPosition);
            rectOfValue.x = startPosition.width * 0.1f;
            rectOfValue.y = (1 + maxHeight) * singleLineHeight;
            rectOfValue.width = startPosition.width * 0.12f;
            rectOfValue.height = singleLineHeight;
            EditorGUI.PropertyField(rectOfValue, value ,GUIContent.none);
            EditorGUI.PropertyField(rectOfInto, into ,GUIContent.none);
            property.serializedObject.ApplyModifiedProperties();
            foreach (var resource in indexForDeleteResource) { resourceAmountCondition.DeleteArrayElementAtIndex(resource); }
            foreach (var technology in indexForDeleteTechnology) { technologyCondition.DeleteArrayElementAtIndex(technology); }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var singleLineHeight = 21;
            Rect startPosition = position;
            startPosition.y = singleLineHeight * 4;
            EditorGUI.BeginProperty(startPosition, label, property);
            DrawGUI( property, startPosition,singleLineHeight);
            EditorGUI.EndProperty();
        }
    }
#endif
}