using System;
using Game.Core.Abstraction;
using Game.Core.Cells;
using Game.Core.DataStructures;
using Game.Services.CellControlling;
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
        private CellService _cellService;
        internal float Distance { get; private set; }

        internal void Initialization(CellService service,  int pool, Vector3 position, float distance, bool invocation =true)
        {
            _cellService = service;
            Distance = distance;
            Position = new CellPosition(pool, position, root,distance);
            transform.position = position;
            _rootedContent = Instantiate(container.initial.root, root);
            State = container.initial.Data;
            FindFarmer();
        }

        internal ICellState MigrateToNewState(ICellState state, bool invocation = true)
        {
            var lastState = State;
            State = state;
            Destroy(_rootedContent);
            _rootedContent = Instantiate(state.Root, root);
            FindFarmer();
            return lastState;
        }
        internal Seed GetSeed() => container.seed;
        internal void DestroyCell()
        {
            Destroy(this.gameObject);
            _cellService.CellDestroy(this);
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