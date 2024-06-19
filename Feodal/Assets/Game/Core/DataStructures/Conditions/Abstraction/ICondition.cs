namespace Game.Core.DataStructures.Conditions.Abstraction
{
    public interface ICondition
    {
        public bool Status();
        public string ConditionName { get; set; }
        public void Initialization();
    }
}