using System;
using System.Threading.Tasks;
using Game.CallBacks.CallbackClick.Button;
using Game.CallBacks.CallbackClick.Simple;
using Game.Cells;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Providers;
using Game.Utility;
using UnityEngine;

namespace Game.UI.Menu
{
    public class UIMenuContainer : MonoBehaviour
    {
        public Transform target;
        public SimpleMenuTypesOpenCallBack menuTypesOpenedCallBack;
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            Proxy.Connect<CellProvider, Cell, UIMenuContainer>(OnUpdateSelect);
            Proxy.Connect<CellAddDetectorProvider, CellAddDetector, CellAddDetector>(PlayerClickedByAddCellObject);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, UIMenuContainer>       (ExitMenu);
            Proxy.Connect<MenuTypesExitProvider, MenuTypes, ButtonExitMenuCallBack>(ExitMenu);
            return Task.CompletedTask;
        }
        private void OnUpdateSelect(Port type, Cell obj) => CloseMenu(); 
        
        private void PlayerClickedByAddCellObject(Port type, CellAddDetector obj)
        {
            OpenMenu();
        }

        private void ExitMenu(Port type, MenuTypes obj)
        {
            if (obj == MenuTypes.ContainerMenu || obj == MenuTypes.BuildingMenu) CloseMenu();
        }
        
        private void OpenMenu()
        {
            menuTypesOpenedCallBack.OnCallBackInvocation?.Invoke(Porting.Type<ButtonOpenMenuCallBack>() ,MenuTypes.ContainerMenu);
            target.gameObject.SetActive(true);
            
            Debugger.Logger("OpenMenu ContainerMenu Menu", ContextDebug.Menu, Process.Action);
        }
        private void CloseMenu()
        {
            target.gameObject.SetActive(false);
            Debugger.Logger("CloseMenu ContainerMenu Menu", ContextDebug.Menu, Process.Action);
        }
    }
}