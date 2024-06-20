using Game.Core.Abstraction;
using Game.Core.Abstraction.UI;
using Game.Core.DataStructures.Conditions.Abstraction.Trades;
using Game.Core.DataStructures.Conditions.TradesConditions;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Technologies.Abstraction;
using Game.Core.DataStructures.Technologies.Base;
using Game.Core.DataStructures.UI.Data;
using UnityEditor;

namespace Game.Core.DataStructures.Editor
{
#if UNITY_EDITOR
    namespace Game.Core.DataStructures.Editor
    {
        [CanEditMultipleObjects]
        [CustomEditor(typeof(Technology))]
        public class TechnologyEditor: AbstractDataStructureEditor<Technology, ITechnologyStore> { }
        
        
        #region Cell
        [CanEditMultipleObjects]
        [CustomEditor(typeof(CellContainer))]
        public class CellContainerEditor : AbstractDataStructureEditor<CellContainer, ICellContainer> { }
        [CanEditMultipleObjects]
        [CustomEditor(typeof(CellState))]
        public class StateCellEditor : AbstractDataStructureEditor<CellState, ICellState> { }
        [CanEditMultipleObjects]
        [CustomEditor(typeof(Resource))]
        public class ResourceEditor : AbstractDataStructureEditor<Resource, IResource> { }
        #endregion
        
        
        #region UI
        [CanEditMultipleObjects]
        [CustomEditor(typeof(UICellContainer))]
        public class UICellContainerEditor : AbstractDataStructureEditor<UICellContainer, IUICellContainer> { }
        [CanEditMultipleObjects]
        [CustomEditor(typeof(UICellContainerElement))]
        public class UICellContainerElementEditor : AbstractDataStructureEditor<UICellContainerElement, IUICellContainerElement> { }
        [CanEditMultipleObjects]
        [CustomEditor(typeof(UIResource))]
        public class UIResourceEditor : AbstractDataStructureEditor<UIResource, IUIResource> { }
        [CanEditMultipleObjects]
        [CustomEditor(typeof(UIResourcesList))]
        public class UIResourcesListEditor : AbstractDataStructureEditor<UIResourcesList, IUIResourceList> { }
        #endregion
        
        
        #region Condition
        // [CustomEditor(typeof(ConditionTradeResourceTechnology))]
        // public class ConditionTradeResourceTechnologyEditor : AbstractDataStructureEditor<ConditionTradeResourceTechnology, ITradeResourceTechnologyCondition> { }
        // [CustomEditor(typeof(ConditionTradeResourceResourceAmount))]
        // public class ConditionTradeResourceResourceAmountEditor : AbstractDataStructureEditor<ConditionTradeResourceResourceAmount, ITradeResourceCondition> { }
        // [CustomEditor(typeof(ConditionTradeBuildTechnology))]
        // public class ConditionTradeBuildTechnologyEditor : AbstractDataStructureEditor<ConditionTradeBuildTechnology,  ITradeBuildingTechnologyCondition> { }
        // [CustomEditor(typeof(ConditionTradeToBuildResourceAmount))]
        // public class ConditionTradeResourceResourceAmount : AbstractDataStructureEditor<ConditionTradeToBuildResourceAmount,  ITradeBuildingCondition> { }
        [CanEditMultipleObjects]
        [CustomEditor(typeof(ConditionTradeSeed))]
        public class ConditionTradeSeedAmountEditor : AbstractDataStructureEditor<ConditionTradeSeed,  ITradeSeedCondition> { }
        // [CanEditMultipleObjects]
        // [CustomEditor(typeof(ConditionTechnologyOtherTechnology))]
        // public class ConditionTradeResourceResourceAmountEditor : AbstractDataStructureEditor<ConditionTechnologyOtherTechnology,   ITechnologyOtherTechnologyCondition> { }
        [CanEditMultipleObjects]
        [CustomEditor(typeof(ConditionTradeResourceAmount))]
        public class ConditionTechnologyResourceAmountEditor : AbstractDataStructureEditor<ConditionTradeResourceAmount,  ITradeResourceCondition> { }
        #endregion

        
        #region Technology
        [CanEditMultipleObjects]
        [CustomEditor(typeof(TradeBuildTechnology))]
        public class TradeBuildTechnologyEditor : AbstractDataStructureEditor<TradeBuildTechnology, ITradeBuildTechnology> { }
        #endregion
        #region Trade
        #endregion
    }
#endif
}