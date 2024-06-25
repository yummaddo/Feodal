using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.Cells;
using Game.DataStructures.UI;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu.BuildingCellMenuList;
using Game.Utility;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UIMenuBuilding : MonoBehaviour
    {
        [SerializeField] private UICellListBuilding listBuilding;
        [SerializeField] private ButtonExitMenuCallBack exitMenuCallBack;
        public Transform target;
        public List<UICellContainer> containers;

        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration( OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            Proxy.Connect<MenuTypesExitProvider, MenuTypes,UIMenuBuilding>(ExitMenu);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes,CellMap>(ExitMenu);
            Proxy.Connect<CellProvider, Cell, CellUpdatedDetector>(OpenMenu);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, UIMenuContainer>(ExitMenu);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>(ExitMenu);
            return Task.CompletedTask;
        }

        private void ExitMenu(Port type, MenuTypes obj)
        {
            if (obj == MenuTypes.BuildingMenu)
                target.gameObject.SetActive(false);
        }
        private void OpenMenu(Port type, Cell cell)
        {
            target.gameObject.SetActive(true);
            var container = cell.container.Data;
            Debugger.Logger($"Open Menu Type {MenuTypes.BuildingMenu}", ContextDebug.Menu , Process.Action);
            OpenMenuWithBase(container.Initial.ExternalName);
        }
        private void OpenMenuWithBase(string nameOfContainer)
        {
            var element = containers.Find(container => container.Data.Container.initial.externalName == nameOfContainer);
            if (element != null)
            {
                target.gameObject.SetActive(true);
                listBuilding.UpdateData(element);
            }
        }
    }
}