using System.Collections.Generic;
using Game.Core.Cells;
using Game.Meta;
using UnityEngine;

namespace Game.Core
{

    public class CellDirection : MonoBehaviour
    {
        [SerializeField] internal List<CellLink> router;
        private SessionStateManager _manager;
    }
}