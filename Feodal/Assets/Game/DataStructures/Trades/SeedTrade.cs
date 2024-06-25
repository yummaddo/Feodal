using System.Collections.Generic;
using Game.DataStructures.Trades.Abstraction;
using Game.DataStructures.Trades.Map;
using Game.Services.CellServices;
using Game.Services.StorageServices.Microservice;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Game.DataStructures.Trades
{
    [System.Serializable]
    public class SeedTrade : AbstractTrade<Resource,ResourceTrade>
    {
        [SerializeField] public Seed into;
        [SerializeField] internal List<ResourceTrade> resourceAmountCondition = new List<ResourceTrade>();
        [SerializeField] internal int stages = 0;
        [SerializeField] internal int currentStage = 0;
        /// <summary>
        /// key start from 1 
        /// </summary>
        internal Dictionary<int, ResourceTrade> Trades = new Dictionary<int, ResourceTrade>();
        private CellService _cellService;
        internal SeedTradeMap Map;
        public override string TradeName => ToString();
        /// <summary>
        /// 
        /// </summary>
        /// <returns> value can be 0 </returns>
        internal int CellQuantity()
        {
            var maximumQuantity = resourceAmountCondition.Count-1;
            return Mathf.Clamp(_cellService.cellMap.GetSeedCount(into), 0, maximumQuantity);
        }
        internal int CurrentStage()
        {
            var maximumStage = resourceAmountCondition.Count;
            return Mathf.Clamp(_cellService.cellMap.GetSeedCount(into) + 1, 1, maximumStage);
        }
        internal void Inject(CellService service)
        {
            _cellService = service;
        }
        public override string ToString() { return $"Seed_{into.title}"; }
        internal override void Initialization(TradeMicroservice microservice)
        {
            base.Initialization(microservice);
            Map = new SeedTradeMap(this,TradeMicroservice);
        }

        protected ResourceTradeMap GetCurrentStageMap() => Map.GetTradeByStage(CellQuantity()).Map;
        public override bool IsTradAble() => TradeMicroservice.CanTrade(GetCurrentStageMap().GetAmount(1));
        public override bool IsTradAble(int amount) => TradeMicroservice.CanTrade(GetCurrentStageMap().GetAmount(amount));
        public override bool IsTradAbleAll() => TradeMicroservice.CanTrade(GetCurrentStageMap().GetAmount(1));
        public override void TradeAmount(int amount) { TradeMicroservice.Trade(this, GetCurrentStageMap().GetAmount(amount), amount); }
        public override void TradeAll() { TradeMicroservice.Trade(this,GetCurrentStageMap().GetAmount(1),1, true); }
        public override void Trade() { TradeMicroservice.Trade(this,GetCurrentStageMap().GetAmount(1),1); }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SeedTrade))]
    public class SeedTradeDrawer : PropertyDrawer
    {
        private int _stageCount = 0;
        private int _currentStage = 0;
        private SerializedProperty _stageCountField;
        private SerializedProperty _currentStageField;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lines = 14;
            return lines * EditorGUIUtility.singleLineHeight;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var singleLineHeight = 21;
            Rect startPosition = position;
            startPosition.y = singleLineHeight * 1;
            
            _stageCountField = property.FindPropertyRelative("stages");
            _currentStageField = property.FindPropertyRelative("currentStage");
            SerializedProperty into = property.FindPropertyRelative("into");
            SerializedProperty resourceTradeList = property.FindPropertyRelative("resourceAmountCondition");
            EditorGUI.BeginProperty(startPosition, label, property);
            
            _currentStage = _currentStageField.intValue-1;
            _stageCount = resourceTradeList.arraySize;
            
            property.serializedObject.ApplyModifiedProperties();
            Rect nextListRect = new Rect(
                startPosition.x + startPosition.width * 0.1f,
                startPosition.y + singleLineHeight*11,
                startPosition.width * 0.1f,
                singleLineHeight);
            Rect prevListRect = new Rect(
                startPosition.x + startPosition.width * 0.22f,
                startPosition.y + singleLineHeight*11,
                startPosition.width * 0.1f,
                singleLineHeight);
            Rect addListRect = new Rect(
                startPosition.x + startPosition.width * 0.34f,
                startPosition.y + singleLineHeight*11,
                startPosition.width * 0.1f,
                singleLineHeight);
            Rect deleteListRect = new Rect(
                startPosition.x + startPosition.width * 0.46f,
                startPosition.y + singleLineHeight*11,
                startPosition.width * 0.1f,
                singleLineHeight);
            Rect resourceAmountConditionRect = new Rect(position);
            resourceAmountConditionRect.y = singleLineHeight * 12;
            if (GUI.Button(nextListRect  , "<<"))
            {
                if (_currentStage != 0)
                {
                    _currentStage--;
                    _currentStageField.intValue--;
                }
                property.serializedObject.ApplyModifiedProperties();
            }            
            if (GUI.Button(prevListRect, ">>"))
            {
                _currentStage++;
                if (_currentStage > _stageCount)
                {
                    _currentStageField.intValue++;
                    // resourceTradeList.arraySize++;
                    // SerializedProperty newElement = resourceTradeList.GetArrayElementAtIndex(resourceTradeList.arraySize - 1);
                    // InstantiateAllInProperty(newElement,resourceTradeList);
                    // property.serializedObject.ApplyModifiedProperties();
                }
                property.serializedObject.ApplyModifiedProperties();
            } 
            if (GUI.Button(addListRect, "+"))
            {
                resourceTradeList.arraySize++;
                SerializedProperty newElement = resourceTradeList.GetArrayElementAtIndex(resourceTradeList.arraySize - 1);
                InstantiateAllInProperty(newElement,resourceTradeList);
                property.serializedObject.ApplyModifiedProperties();
            }
            if (GUI.Button(deleteListRect, "-"))
            {
                if (_stageCount != 0 && _stageCount != 1)
                {
                    resourceTradeList.DeleteArrayElementAtIndex(_currentStage);
                    if (_stageCount - 1 == _currentStage)
                    {
                        _currentStage--;                        
                    }
                    _stageCount--;
                }
                property.serializedObject.ApplyModifiedProperties();
            }
            Rect intoListRect = new Rect(
                startPosition.x,
                startPosition.y+ singleLineHeight*9,
                startPosition.width * 0.5f,
                singleLineHeight);
            Rect currentStageListRect = new Rect(
                startPosition.x + startPosition.width * 0.5f,
                startPosition.y + singleLineHeight*9,
                startPosition.width * 0.25f,
                singleLineHeight);
            Rect maxStageListRect = new Rect(
                startPosition.x + startPosition.width * 0.75f,
                startPosition.y+ singleLineHeight*9,
                startPosition.width * 0.25f,
                singleLineHeight);
            EditorGUI.PropertyField(intoListRect, into, GUIContent.none);
            EditorGUI.PropertyField(currentStageListRect, _currentStageField, GUIContent.none);
            EditorGUI.PropertyField(maxStageListRect, _stageCountField,  GUIContent.none);
            if (resourceTradeList.arraySize <= 0 || _currentStage >= resourceTradeList.arraySize || _currentStage<0)
            {
                if (_currentStage < 0)
                {
                    _currentStage = 0;
                }
                resourceTradeList.arraySize++;
                var newElement = resourceTradeList.GetArrayElementAtIndex(resourceTradeList.arraySize - 1);
                InstantiateAllInProperty(newElement,resourceTradeList);
                property.serializedObject.ApplyModifiedProperties();
            }
            else
            {
                var trade = resourceTradeList.GetArrayElementAtIndex(_currentStage);
                ResourceTradeDrawer.DrawGUI(trade,resourceAmountConditionRect,singleLineHeight );
            }
            _currentStageField.intValue = _currentStage+1;
            _stageCountField.intValue = _stageCount;
            EditorGUI.EndProperty();
            property.serializedObject.ApplyModifiedProperties();
        }
        private void InstantiateAllInProperty(SerializedProperty property,SerializedProperty main)
        {
            var technologyCondition = property.FindPropertyRelative("technologyCondition");
            var resourceAmountCondition = property.FindPropertyRelative("resourceAmountCondition");
            var into = property.FindPropertyRelative("Into");
            var value = property.FindPropertyRelative("Value");
            if (main.arraySize >= 2)
            {
                var prev = main.GetArrayElementAtIndex(main.arraySize - 2);
                var t = prev.FindPropertyRelative("technologyCondition");
                var r = prev.FindPropertyRelative("resourceAmountCondition");
                var it = prev.FindPropertyRelative("Into");
                var v = prev.FindPropertyRelative("Value");
                for (int i = 0; i < t.arraySize; i++)
                {
                    var tempT = technologyCondition.GetArrayElementAtIndex(i);
                    var soursT = t.GetArrayElementAtIndex(i);
                    tempT.objectReferenceValue = soursT.objectReferenceValue;
                }
                resourceAmountCondition.arraySize = r.arraySize;
                for (int i = 0; i < r.arraySize; i++)
                {
                    var tempR = resourceAmountCondition.GetArrayElementAtIndex(i);
                    var soursR = r.GetArrayElementAtIndex(i);
                    tempR.FindPropertyRelative("value").intValue = soursR.FindPropertyRelative("value").intValue;
                    tempR.FindPropertyRelative("resource").objectReferenceValue = soursR.FindPropertyRelative("resource").objectReferenceValue;
                }
                into.objectReferenceValue = it.objectReferenceValue;
                value.intValue = v.intValue;
            } else
            {
                if (technologyCondition != null)
                {
                    if (technologyCondition.arraySize == 0)
                    {
                        technologyCondition.arraySize = 1;
                        technologyCondition.GetArrayElementAtIndex(0).objectReferenceValue = null;
                    }
                }
                if (resourceAmountCondition != null)
                {
                    if (resourceAmountCondition.arraySize == 0)
                    {
                        resourceAmountCondition.arraySize = 1; 
                        var newElement = resourceAmountCondition.GetArrayElementAtIndex(0);
                        newElement.FindPropertyRelative("value").intValue = 0; 
                        newElement.FindPropertyRelative("resource").objectReferenceValue = null;
                    }
                }
                // Ініціалізуємо Resource Into
                if (into != null)
                    if (into.objectReferenceValue == null)
                        into.objectReferenceValue = null; 
                if (value != null && value.propertyType == SerializedPropertyType.Integer)
                    value.intValue = 0; 
            }
        }
    }
#endif
}