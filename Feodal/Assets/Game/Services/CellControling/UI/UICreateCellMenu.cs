using Game.Meta;
using Game.Services.CellControling.Microservice;
using UnityEngine;

namespace Game.Services.CellControling.UI
{
    public class UICreateCellMenu : MonoBehaviour
    {
        public GameObject createCellMenu;
        private CellCreateMicroservice _cellCreateMicroservice;
        public void Awake()
        {
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += Inject;
        }
        private void Inject()
        {
            _cellCreateMicroservice = SessionStateManager.Instance.Container.Resolve<CellCreateMicroservice>();
            _cellCreateMicroservice.OnServiceCellCreateColed += OnServiceCellCreateColed;
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession -= Inject;
        }

        private void OnServiceCellCreateColed()
        {
            createCellMenu.SetActive(true);
        }
    }
}