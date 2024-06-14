using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Cell
{

    public class CellDirection : MonoBehaviour
    {
        [SerializeField] internal List<CellLink> router;
    }
}