using UnityEngine;

namespace Game.Core.DataStructures.Technologies.Abstraction
{
    public interface ICellTechnology : ITechnology
    {
        [field: SerializeField] public CellContainer Container { get; set; }
    }
}