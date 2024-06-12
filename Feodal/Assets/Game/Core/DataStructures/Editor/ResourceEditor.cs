#if UNITY_EDITOR
namespace Game.Core.DataStructures.Editor
{
    using Abstraction;
    using UnityEditor;
    [CustomEditor(typeof(Resource))]
    public class ResourceEditor : AbstractDataStructureEditor<Resource, IResource>
    {
    }
}
#endif