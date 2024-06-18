using System.Collections.Generic;
using Game.Core.Abstraction;
using UnityEngine;

namespace Game.Core.DataStructures.Technology
{
    public abstract class AbstractTechnology<TTechnologyType>: AbstractDataStructure<TTechnologyType>, ITechnology
        where TTechnologyType : ITechnology
    {
        public string TechnologyName => DataNamePattern;
        public abstract void Trade();
        public abstract bool Status();
        internal abstract void Initialization();
        public List<ICondition> ConditionsToResearch { get; set; }
        public TTechnologyType GetTechnology() => CompareTemplate();
    }
}