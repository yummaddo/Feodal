using Game.Core.Abstraction;
using Game.Core.DataStructures;
using UnityEngine;

namespace Game.Core
{
    public class Cell : MonoBehaviour
    {
        public int poolId = 0;
        public ICellPosition Position;
        public CellVisualSelection selection;
        internal bool IsBaseState => container.initial.externalName == State.ExternalName;
        internal ICellState State;
        [SerializeField] internal Transform root;
        [SerializeField] internal CellDirection direction;
        [SerializeField] internal CellContainer container;
        
        private GameObject _rootedContent;
        internal void Initialization(int pool, Vector3 position, float distance)
        {
            Position = new CellPosition(pool, position, root,distance);
            transform.position = position;
            _rootedContent = Instantiate(container.initial.root, root);
            State = container.initial.Data;
        }
        
        internal void MigrateToNewState(ICellState state)
        {
            State = state;
            Destroy(_rootedContent);
            _rootedContent = Instantiate(state.Root, root);
        }
    }
}