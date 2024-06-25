using Game.DataStructures;
using Game.DataStructures.Abstraction;
using Game.Services.CellServices;
using UnityEngine;

namespace Game.Cells
{
    public class Cell : MonoBehaviour
    {
        public int poolId = 0;
        public ICellPosition Position;
        public CellVisualSelection selection;
        internal bool IsBaseState => containerName == stateName;
        public string containerName;
        public string stateName;
        internal ICellState State;
        [SerializeField] internal Transform root;
        [SerializeField] internal CellDirection direction;
        [SerializeField] internal CellUpdatedDetector detector;
        [SerializeField] internal CellContainer container;
        private GameObject _rootedContent;
        private CellService _cellService;
        internal float Distance { get; private set; }

        internal void Initialization(CellService service,  int pool, Vector3 position, float distance, bool invocation =true)
        {
            _cellService = service;
            Distance = distance;
            Position = new CellPosition(pool, position, root,distance);
            transform.position = position;
            _rootedContent = Instantiate(container.initial.root, root);
            containerName = container.initial.Data.ExternalName;
            stateName = container.initial.Data.ExternalName;
            State = container.initial.Data;
            gameObject.name = $"Cell:{Position.CellHexCoord}:{stateName}{containerName}";
            if (detector) detector.Init(this);
            FindFarmer();
        }
        internal ICellState MigrateToBase(bool invocation = true)
        {
            var lastState = State;
            State = container.initial.Data;
            Destroy(_rootedContent);
            stateName = container.containerName;
            _rootedContent = Instantiate(container.initial.Data.Root, root);
            if (detector) detector.Init(this);
            FindFarmer();
            gameObject.name = $"Cell:{Position.CellHexCoord}:{stateName}:{containerName}";
            return lastState;
        }
        internal ICellState MigrateToNewState(ICellState state, bool invocation = true)
        {
            var lastState = State;
            State = state;
            Destroy(_rootedContent);
            stateName = state.ExternalName;
            _rootedContent = Instantiate(state.Root, root);
            if (detector) detector.Init(this);
            FindFarmer();
            gameObject.name = $"Cell:{Position.CellHexCoord}:{stateName}{containerName}";
            return lastState;
        }
        internal Seed GetSeed() => container.seed;
        internal void DestroyCell()
        {
            Destroy(this.gameObject);
        }
        private void FindFarmer()
        {
            if (!IsBaseState)
            {
                var farmer = _rootedContent.GetComponent<CellResourceFarmer>();
                if (farmer) farmer.Initialization(this);
            }
        }
    }
}