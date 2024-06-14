using Game.Core.DataStructures.UI.Data;

#if UNITY_EDITOR
namespace Game.Core.DataStructures.Editor.UI
{
    using Game.Core.Abstraction.UI;
    using Game.Core.DataStructures.UI;
    using UnityEditor;
    [CustomEditor(typeof(UICellContainerElement))]
    public class UICellContainerElementEditor : AbstractDataStructureEditor<UICellContainerElement, IUICellContainerElement>
    {
    }
}
#endif