using System.Collections.Generic;
using Game.Typing;
using UnityEngine;

namespace Game.DataStructures.Abstraction
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