#if UNITY_EDITOR
namespace Game.Core.DataStructures.Editor.UI
{
    using Game.Core.DataStructures.UI.Data;
    using Game.Core.Abstraction.UI;
    using UnityEditor;
    [CustomEditor(typeof(UIResource))]
    public class UIResourceEditor : AbstractDataStructureEditor<UIResource, IUIResource>
    {
    }
}
#endif