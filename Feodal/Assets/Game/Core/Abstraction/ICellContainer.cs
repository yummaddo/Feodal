using System.Collections.Generic;

namespace Game.Core.Abstraction
{
    public interface ICellContainer
    {
        public ICellState Initial { get; set; }
        public List<ICellState> States { get; set; }
    }
}