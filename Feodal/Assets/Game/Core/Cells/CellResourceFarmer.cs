using System;
using System.Collections;
using System.Collections.Generic;
using Game.Core.DataStructures;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;
using UnityEngine;

namespace Game.Core.Cells
{
    public class CellResourceFarmer : MonoBehaviour
    {
        [SerializeField] private SimpleClickCallback<CellResourcePackaging> clickCallback;
        [SerializeField] internal int farmAmountPerIteration = 1;
        [SerializeField] float farmTime = 2f;
        [SerializeField] internal Resource resource;
        
        private Cell _cellConnected;
        private Coroutine _farmingCoroutine;
        private int _farmedValue = 0;
        private int _farmedIterations = 0;

        public void Initialization(Cell cell)
        {
            _cellConnected = cell;
            _farmingCoroutine = StartCoroutine(FarmResource());
            Proxy.Connect<CellSelectProvider, Cell>(OnCellClicked);
        }
        private void OnDisable()
        {
            if (_farmingCoroutine != null) StopCoroutine(_farmingCoroutine);
        }
        private void OnCellClicked(Cell cell)
        {
            try
            {
                if (_cellConnected.Position.CellHexCoord == cell.Position.CellHexCoord)
                {
                    GetAllResource();
                }
            }
            catch (Exception e)
            {
                Debugger.Logger(e.Message, Process.TrashHold);
            }
        }
        private IEnumerator FarmResource() {
            while (true)
            {
                yield return new WaitForSeconds(farmTime); 
                Farm();
            } 
        }

        private void GetAllResource()
        {
            clickCallback.OnClick.Invoke(new CellResourcePackaging(resource.Data, _farmedValue));
            _farmedValue = 0;
            _farmedIterations = 0;
        }
        
        private void Farm()
        {
            if (resource != null)
            {
                
                _farmedValue += Math.Clamp(farmAmountPerIteration - farmAmountPerIteration * (_farmedIterations / resource.quantity),0, Int32.MaxValue);
                _farmedIterations++;
                
            }
        }
    }
}