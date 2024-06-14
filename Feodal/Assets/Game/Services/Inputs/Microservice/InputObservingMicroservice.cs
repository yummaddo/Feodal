using System.Collections;
using Game.Services.Abstraction.MicroService;
using UnityEngine;

namespace Game.Services.Inputs.Microservice
{
    public class InputObservingMicroservice : AbstractMicroservice<InputService>
    {
        [SerializeField] private float daley = 0.015f;
        protected override void OnAwake()
        {
        }

        protected override void OnStart()
        {
            // _updateCallCoroutine = StartCoroutine(UpdateCallCoroutine());
        }
        
        protected override void Stop()
        {
            
        }
        protected override void ReStart()
        {
            
        }
        private bool _pressedClickButtonStatus = false;
        private bool _clickOnUI = false;
        private Coroutine _updateCallCoroutine;

        private void Start()
        {
        }

        private void OnDestroy()
        {
            if (_updateCallCoroutine != null)
            {
                StopCoroutine(_updateCallCoroutine);
                _updateCallCoroutine = null;
            }
        }

        private IEnumerator UpdateCallCoroutine()
        {
            while (true)
            {
                if (StartClicked())
                {
                    if (!_clickOnUI)
                        Service.ClickEnteredVoid();
                }
                else if (ProcessedClicked())
                {
                    if (!_clickOnUI)
                        Service.ClickPrecessedVoid();
                }
                else if (EndClicked())
                {
                    if (!_clickOnUI)
                        Service.ClickCloseVoid();
                    Reset();
                }
                else if (_clickOnUI && EndClicked())
                {
                    _clickOnUI = false;
                }

                var time = Time.deltaTime / Time.timeScale;
                yield return new WaitForSecondsRealtime(time);
            }
        }
        
        private bool StartClicked()
        {
            if (Input.GetButton("Fire1") && !_pressedClickButtonStatus)
            {
                _pressedClickButtonStatus = true;
                _clickOnUI = RayCastToUI();
                return true;
            }
            return false;
        }
        private bool ProcessedClicked()
        {
            return Input.GetButton("Fire1") && _pressedClickButtonStatus;
        }
        private bool EndClicked()
        {
            return !Input.GetButton("Fire1") && _pressedClickButtonStatus;
        }
        private bool RayCastToUI()
        {
            var myObjectLayer = LayerMask.NameToLayer("UI");
            int layerMask = 1 << myObjectLayer;
            Ray ray = Service.mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100.0f, layerMask))
                if (hit.point != Vector3.zero) return true;
            return false;
        }
        private void Reset()
        {
            _pressedClickButtonStatus = false;
            _clickOnUI = false;
        }
    }
}