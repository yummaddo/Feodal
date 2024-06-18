using Game.Core;
using Game.Core.Cells;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.Providers;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UIMenuContainer : MonoBehaviour
    {
        public Transform target;
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += OnSceneAwakeMicroServiceSession;
        }
        private void OnSceneAwakeMicroServiceSession()
        {
            Proxy.Connect<CellAddProvider, CellAddDetector>(PlayerClickedByAddCellObject);
            Proxy.Connect<MenuExitProvider, MenuTypes>(PlayerClickedByAddExitMenu);
            Proxy.Connect<CellUpdateProvider, Cell>(OnUpdateSelect);
        }

        private void OnUpdateSelect(Cell obj)
        {
            CloseMenu();
        }

        private void PlayerClickedByAddExitMenu(MenuTypes obj)
        {
            if (obj == MenuTypes.ContainerMenu || obj == MenuTypes.BuildingMenu) 
                CloseMenu();
        }
        private void PlayerClickedByAddCellObject(CellAddDetector obj) => OpenMenu();
        private void OpenMenu() => target.gameObject.SetActive(true);
        private void CloseMenu() => target.gameObject.SetActive(false);
    }
    
}