using System.Collections.Generic;
using Game.Core.Typing;
using UnityEngine;

namespace Game.Core.Abstraction
{
    public interface ICellState
    {
        public int ID { get; set; }
        public string ExternalName { get; set; }

        public CellType Type { get; set; }
        public CellActiveType ActiveType { get; set; }

        public GameObject Root { get; set; }
        public List<ICellState> SwitchList { get; set; }

        public IResource Resource { get; set; }
        public ICellState Base { get; set; }
        public ICellTransition Transition { get; set; }
    }
}