using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.DataStructures
{
    [CreateAssetMenu(menuName = "Seed")]
    public class Seed : Resource
    {
        [SerializeField] internal Sprite image;
        public List<CellState> dependentStates  = new List<CellState>();
    }
}