using System.Collections.Generic;
using UnityEngine;

namespace Game.Cells
{

    public class CellDirection : MonoBehaviour
    {
        [SerializeField] internal List<CellLink> router;
        private SessionLifeStyleManager _manager;
    }
}