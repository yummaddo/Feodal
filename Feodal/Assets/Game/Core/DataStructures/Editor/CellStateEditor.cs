#if UNITY_EDITOR
namespace Game.Core.DataStructures.Editor
{
    using Abstraction;
    using UnityEditor;
    [CustomEditor(typeof(CellState))]
    public class StateCellEditor : AbstractDataStructureEditor<CellState, ICellState>
    {
    }
}
#endif