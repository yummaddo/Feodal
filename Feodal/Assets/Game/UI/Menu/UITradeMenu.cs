using Game.Core.Abstraction;
using Game.Meta;
using Game.Services.Proxies;
using Game.Services.Proxies.ClickCallback.Button;
using Game.Services.Proxies.Providers;
using Game.UI.Menu.ResourceListMenu;
using Game.UI.Menu.TradeMenu;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menu
{
    public class UITradeMenu : MonoBehaviour
    {


        [SerializeField] private UITradeResourceListController controller;
        private void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += OnSceneAwakeMicroServiceSession;
        }
        private void OnSceneAwakeMicroServiceSession()
        {
            Proxy.Connect<UIListResourceElementProvider, UIResourceListElement, UIResourceListElement>(OnClickedBySimpleResource);
        }
        private void OnClickedBySimpleResource(Port port,UIResourceListElement element)
        {
            controller.View(element);
        }
    }
}