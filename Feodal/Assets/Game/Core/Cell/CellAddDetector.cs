using System;
using Game.Meta;
using Game.Services.Inputs;
using UnityEngine;

namespace Game.Core.Cell
{
    public class CellAddDetector : MonoBehaviour
    {
        private bool _isClick = false;
        private float _time = 0.0f;
        private InputService _service;
        private SessionStateManager _manager;
        public CellLink link;

        private void Awake()
        {
            _manager= SessionStateManager.Instance;
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession += Inject;
        }
        private void Inject()
        {
            _service = SessionStateManager.Instance.Container.Resolve<InputService>();
            SessionStateManager.Instance.OnSceneAwakeMicroServiceSession -= Inject;
        }

        public void OnMouseDown()
        {
            _isClick = true;
        }
        public void OnMouseDrag()
        {
            _time += Time.deltaTime;
        }
        public void OnMouseUp()
        {
            _time += Time.deltaTime;
            if (_isClick)
            {
                _service.ClickedByAddCellObject(gameObject,this);
            }
        }
        public void OnMouseExit()
        {
            _isClick = false;
        }
    }
}