using System.Collections.Generic;
using Game.Core.Abstraction;
using UnityEngine;

namespace Game.Core.DataStructures
{
    [CreateAssetMenu(menuName = "Seed")]
    public class Seed : Resource
    {
        [SerializeField] internal Sprite image;
        public List<CellState> dependentStates  = new List<CellState>();
        public bool Contains(ICellState state)
        {
            foreach (var stateDependent in dependentStates)
            {
                if (state.ExternalName == stateDependent.Data.ExternalName)
                    return true;
            }
            return false;
        }
        public bool Is(Seed state)
        {
            if (state != null)
            {
                return state.Data.Title == title;
            }
            return false;
        }
    }
}