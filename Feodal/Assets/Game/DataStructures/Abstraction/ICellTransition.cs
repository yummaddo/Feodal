namespace Game.DataStructures.Abstraction
{
    public interface ICellTransition
    {
        public bool Last { get; }
        public bool Default { get; }
        public bool CanChangeState(ICellState from, ICellState to);
    }
}