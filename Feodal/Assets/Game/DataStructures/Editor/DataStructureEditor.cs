using Game.DataStructures.Abstraction;
using Game.DataStructures.UI;
using Game.UI.Abstraction;
using UnityEditor;

namespace Game.DataStructures.Editor
{
#if UNITY_EDITOR
    namespace Game.Core.DataStructures.Editor
    {
        // [CanEditMultipleObjects]
        // [CustomEditor(typeof(Technology))]
        // public class TechnologyEditor: AbstractDataStructureEditor<Technology, ITechnologyStore> { }
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
        
        [CanEditMultipleObjects]
        [CustomEditor(typeof(UISeedList))]
        public class UISeedListEditor : AbstractDataStructureEditor<UISeedList, IUIResourceList> { }
        [CanEditMultipleObjects]
        [CustomEditor(typeof(UISeed))]
        public class UISeedEditor : AbstractDataStructureEditor<UISeed, UISeed> { }
        #endregion
    }
#endif
}