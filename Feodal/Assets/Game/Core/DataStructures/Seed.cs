using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.DataStructures
{
    [CreateAssetMenu(menuName = "Seed")]
    public class Seed : Resource
    {
        public List<CellState> dependentStates  = new List<CellState>();
    }
}