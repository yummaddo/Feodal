using System.Collections.Generic;
using Game.Core.Typing;

namespace Game.Core.Abstraction
{
    public interface ICellContainer
    {
        public int Price { get; set; }
        public CellSeedType SeedType { get; set; }
        public ICellState Initial { get; set; }
        public List<ICellState> States { get; set; }
    }
}