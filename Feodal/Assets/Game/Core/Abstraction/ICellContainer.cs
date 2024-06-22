using System.Collections.Generic;
using Game.Core.Typing;
using UnityEngine;

namespace Game.Core.Abstraction
{
    public interface ICellContainer : ISeed
    {
        public GameObject CellTemplate { get; set; }
        public int Price { get; set; }
        public CellSeedType SeedType { get; set; }
        public ICellState Initial { get; set; }
        public List<ICellState> States { get; set; }
    }
}