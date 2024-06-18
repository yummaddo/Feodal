
#if UNITY_EDITOR
namespace Game.Core.DataStructures.Editor.UI
{
    using Game.Core.DataStructures.UI.Data;
    using Game.Core.Abstraction.UI;
    using UnityEditor;
    [CustomEditor(typeof(UIResourcesList))]
    public class UIResourcesListEditor : AbstractDataStructureEditor<UIResourcesList, IUIResourceList>
    {
    }
}
#endif