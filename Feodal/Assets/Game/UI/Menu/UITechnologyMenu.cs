using System;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.UI.Menu.TechnologyMenu;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UITechnologyMenu :  MonoBehaviour
    {
        [SerializeField] private UITechnologyController controller;
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, UITechnologyMenu>(OnClickedByMenuExit);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>(ExitMenu);
            return Task.CompletedTask;
        }

        private void ExitMenu(Port arg1, MenuTypes arg2)
        {
            OnClickedByMenuExit(arg1, arg2);
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