using System.Collections.Generic;
using Game.DataStructures.Abstraction;
using Game.Typing;
using UnityEngine;

namespace Game.DataStructures
{
    [CreateAssetMenu(menuName = "CellState")]
    public class CellState : AbstractDataStructure<ICellState>, ICellState
    {
        protected override ICellState CompareTemplate()
        {
            ID = id;
            ExternalName = externalName;
            Type = type;
            ActiveType = activeType;
            Root = root;
            SwitchList = new List<ICellState>();
            foreach (var switcher in switchList) { SwitchList.Add(switcher); }
            Resource = resource;
            Base = baseState;
            return this;
        }

        public override string ToString()
        {
            return externalName;
        }
        internal override string DataNamePattern => $"{type}_{activeType}_State_{id}_{externalName}";
        public int id;
        public string externalName;
        public CellType type;
        public CellActiveType activeType;
        public GameObject root;
        public List<CellState> switchList = new List<CellState>();
        public Resource resource;
        public CellState baseState;
        public ICellTransition transition;
        #region ICellState

            public int ID { get; set; }
            public string ExternalName { get; set; }
            public CellType Type { get; set; }
            public CellActiveType ActiveType { get; set; }
            public GameObject Root { get; set; }
            public List<ICellState> SwitchList { get; set; }
            public IResource Resource { get; set; }
            public ICellState Base { get; set; }
            public ICellTransition Transition { get; set; }

        #endregion
    }
}