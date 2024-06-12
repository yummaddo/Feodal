#if UNITY_EDITOR
namespace Game.Core.DataStructures.Editor
{
    using Abstraction;
    using UnityEditor;
    [CustomEditor(typeof(CellContainer))]
    public class CellContainerEditor : AbstractDataStructureEditor<CellContainer, ICellContainer>
    {
    }
}
#endif