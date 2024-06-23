using Game.Core.Abstraction;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.Providers;
using Game.UI.Menu.TechnologyMenu;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UITechnologyMenu :  MonoBehaviour
    {
        [SerializeField] private UITechnologyController controller;
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += OnSceneAwakeMicroServiceSession;
        }
        private void OnSceneAwakeMicroServiceSession()
        {
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>(OnClickedByMenuExit);
        }
        private void OnClickedByMenuExit(Port arg1, MenuTypes arg2)
        {
            if (arg2 == MenuTypes.Technology)
            {
                controller.CloseMenuManaged();
            }
        }
    }
}