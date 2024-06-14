using Game.Core.DataStructures;
using UnityEngine;

namespace Game.Core.Cell
{
    public class Cell : MonoBehaviour
    {
        public int poolId = 0;
        [SerializeField] internal Transform root;
        [SerializeField] internal CellDirection direction;
        [SerializeField] internal CellContainer container;
    }

}