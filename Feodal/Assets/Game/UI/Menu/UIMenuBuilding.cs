using System.Collections.Generic;
using Game.Core.Abstraction;
using Game.Core.Cells;
using Game.Core.DataStructures.UI.Data;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.Providers;
using Game.UI.Menu.BuildingCellMenuList;
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
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += OnSceneAwakeMicroServiceSession;
        }
        private void OnSceneAwakeMicroServiceSession()
        {
            Proxy.Connect<MenuCellUpdateProvider, ICellContainer>(OpenMenu);
            Proxy.Connect<MenuExitProvider, MenuTypes>(ExitMenu);
                ;
        }

        private void ExitMenu(MenuTypes obj)
        {
            if (obj == MenuTypes.BuildingMenu)
            {
                target.gameObject.SetActive(false);
            }
        }

        private void OpenMenu(ICellContainer obj)
        {
            target.gameObject.SetActive(true);
            Debug.Log("OpenMenu");
            OpenMenuWithBase(obj.Initial.ExternalName);
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