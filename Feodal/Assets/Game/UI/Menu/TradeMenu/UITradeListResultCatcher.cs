using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.UI.Menu.TradeMenu
{
    public class UITradeListResultCatcher : MonoBehaviour
    {
        [SerializeField] private UITradeListController controller;
        private void Awake()
        {
            SessionLifeStyleManager.AddLifeIteration(OnSceneAwakeMicroServiceSession, SessionLifecycle.OnSceneAwakeMicroServiceSession);
        }
        private Task OnSceneAwakeMicroServiceSession(IProgress<float> progress)
        {
            // _service = SessionStateManager.Instance.ServiceLocator.Resolve<StorageService>();
            // _tradeMicroservice = SessionStateManager.Instance.ServiceLocator.Resolve<TradeMicroservice>();
            // Proxy.Connect<DatabaseResourceProvider,ResourceTempedCallBack,ResourceTempedCallBack>(SomeResourceUpdate);
            return Task.CompletedTask;
        }
    }
}