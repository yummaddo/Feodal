using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.Typing;
using UnityEngine;

namespace Game.Core.DataStructures
{
    [CreateAssetMenu(menuName = "CellContainer")]
    public class CellContainer : AbstractDataStructure<ICellContainer>, ICellContainer
    {
        public string containerName;
        public int price;
        public CellSeedType seedType;
        public CellState initial;
        public List<CellState> states;
        protected override ICellContainer CompareTemplate()
        {
            Initial = initial;
            States = new List<ICellState>();
            foreach (var state in states) { States.Add(state); }
            Price = price;
            SeedType = seedType;
            return this;
        }
        protected override string DataNamePattern => $"Cell_Container_{containerName}";

        #region ICellContainer

            public int Price { get; set; }
            public CellSeedType SeedType { get; set; }
            public ICellState Initial { get; set; }
            public List<ICellState> States { get; set; }
        #endregion
    }
}