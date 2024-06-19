namespace Game.Core.DataStructures.Technologies.Abstraction
{
    public interface ITechnology
    {
        public string Title  { get; set; }
        public bool CurrentStatus { get; set; }
        public bool Status();
    }
}