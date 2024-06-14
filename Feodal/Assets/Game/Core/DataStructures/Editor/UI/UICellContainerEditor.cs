using Game.Core.DataStructures.UI.Data;

#if UNITY_EDITOR
namespace Game.Core.DataStructures.Editor.UI
{
    using Game.Core.Abstraction.UI;
    using Game.Core.DataStructures.UI;
    using UnityEditor;
    [CustomEditor(typeof(UICellContainer))]
    public class UICellContainerEditor : AbstractDataStructureEditor<UICellContainer, IUICellContainer>
    {
    }
}
#endif