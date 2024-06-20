using System;
using Game.Core.DataStructures;
using Game.Services.Proxies.ClickCallback.Abstraction;
using UnityEngine;

namespace Game.Core.Cells
{
    public class CellResourceFarmer : MonoBehaviour
    {
        [SerializeField] private SimpleClickCallback<Cell> clickCallback;
        [SerializeField] private int click;
        [SerializeField] private Resource resource;
        private void OnEnable()
        {
            clickCallback.OnClick += Upped;
        }
        private void OnDisable()
        {
            clickCallback.OnClick -= Upped;
        }
        private void Upped(Cell cell)
        {
            
        }
    }
}