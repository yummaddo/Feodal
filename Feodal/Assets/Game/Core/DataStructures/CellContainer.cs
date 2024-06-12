using System.Collections.Generic;
using Game.Core.Abstraction;
using UnityEngine;

namespace Game.Core.DataStructures
{
    [CreateAssetMenu(menuName = "CellContainer")]
    public class CellContainer : AbstractDataStructure<ICellContainer>, ICellContainer
    {
        public string containerName;
        public CellState initial;
        public List<CellState> states;
        protected override ICellContainer CompareTemplate()
        {
            Initial = initial;
            States = new List<ICellState>();
            foreach (var state in states) { States.Add(state); }

            return this;
        }
        
        protected override string DataNamePattern => $"Cell_Container_{containerName}";

        #region ICellContainer
            public ICellState Initial { get; set; }
            public List<ICellState> States { get; set; }
        #endregion
    }
}