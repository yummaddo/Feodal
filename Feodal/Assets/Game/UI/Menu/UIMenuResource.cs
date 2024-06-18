using Game.Core.Abstraction;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.Providers;
using Game.UI.Menu.ResourceListMenu;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UIMenuResource : MonoBehaviour
    {
        public Transform target;
        [SerializeField] private UIResourceListController controller;
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += OnSceneAwakeMicroServiceSession;
        }
        private void OnSceneAwakeMicroServiceSession()
        {
            Proxy.Connect<UniversalResourceProvider, IResource>(OnClickedByUniversalResource);
            Proxy.Connect<MenuExitProvider, MenuTypes>(OnClickedByMenuExit);
        }
        private void OnClickedByMenuExit(MenuTypes obj)
        {
            if (obj == MenuTypes.ResourceMenu)
            {
                CloseMenu();
            }
        }
        private void CloseMenu()
        {
            controller.Clear();
            target.gameObject.SetActive(false);
        }
        private void OnClickedByUniversalResource(IResource resource)
        {
            target.gameObject.SetActive(true);
            controller.Clear();
            controller.ViewList(controller.ResourcesListCompare[resource]);
        }
    }
}