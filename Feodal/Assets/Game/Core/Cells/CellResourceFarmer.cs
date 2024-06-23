using System;
using System.Collections;
using Game.Core.DataStructures;
using Game.Core.DataStructures.UI.Data;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Simple;
using Game.Services.Proxies.Providers;
using Game.UI.Animation;
using UnityEngine;

namespace Game.Core.Cells
{
    public class CellResourceFarmer : MonoBehaviour
    {
        
        [SerializeField] private SimpleCellResourcePackagingCallBack clickCallback;
        
        [Range(1f,20f)]
        [SerializeField] 
        internal int farmAmountPerIteration = 1;
        
        [Range(0.4f,50f)]
        [SerializeField] float farmTime = 2f;
        
        [SerializeField] internal UIResource resource;
        [SerializeField] internal UICellFarmerProgressBar progressBar;
        private Cell _cellConnected;
        private Coroutine _farmingCoroutine;
        private int _farmedValue = 0;
        private int _farmedIterations = 0;

        public void Initialization(Cell cell)
        {
            _cellConnected = cell;
            _farmingCoroutine = StartCoroutine(FarmResource());
            Debugger.Logger($"CellResourceFarmer {_cellConnected}", Process.Create );
            Proxy.Connect<CellProvider, Cell, CellResourceFarmer>(OnCellClicked);
            progressBar.Init(resource.resourceImage);
        }
        private void OnDisable()
        {
            if (_farmingCoroutine != null) StopCoroutine(_farmingCoroutine);
        }
        private void OnCellClicked(Port type , Cell cell)
        {
            // try
            // {
                if (_cellConnected.Position.CellHexCoord == cell.Position.CellHexCoord)
                {
                    if (_farmedValue > 0)
                    {
                        GetAllResource();
                    }
                    else
                    {
                        progressBar.ResourceNone();
                    }
                }
            // }
            // catch (Exception e)
            // {
                // Debugger.Logger(e.Message, Process.TrashHold);
            // }
        }
        private IEnumerator FarmResource()
        {
            float currentTime = 0.0f;
            while (true)
            {
                yield return new WaitForEndOfFrame();
                currentTime += Time.deltaTime;
                progressBar.SetFarmFrame(currentTime/farmTime);
                if (currentTime > farmTime)
                {
                    currentTime %= farmTime;
                    Farm();
                }
            } 
        }

        private void GetAllResource()
        {
            Debugger.Logger($"CellResourceFarmer Farm {resource.resource.Data}= {_farmedValue}", Process.Update );
            clickCallback.OnCallBackInvocation.Invoke(Porting.Type<CellResourceFarmer>(), new CellResourcePackaging(resource.resource.Data, _farmedValue));
            progressBar.GetResource(_farmedValue);
            _farmedValue = 0;
            progressBar.SetFarmValue(_farmedValue);
            _farmedIterations = 0;
        }
        private void Farm()
        {
            if (resource != null)
            {
                _farmedValue += Math.Clamp(farmAmountPerIteration - farmAmountPerIteration * (_farmedIterations / resource.resource.Data.Quantity),0, Int32.MaxValue);
                _farmedIterations++;
                progressBar.SetFarmValue(_farmedValue);
                progressBar.SetFarmFrame(0);
            }
        }
    }
}