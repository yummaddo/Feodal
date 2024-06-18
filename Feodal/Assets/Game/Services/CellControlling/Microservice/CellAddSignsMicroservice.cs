using System.Collections;
using Game.Services.Abstraction.MicroService;
using UnityEngine;

namespace Game.Services.CellControlling.Microservice
{
    public class CellAddSignsMicroservice : AbstractMicroservice<CellService>
    {
        [SerializeField] private Material singMaterial;
        
        public AnimationCurve animatedMaterialWay;
        public float cycleTime = 2;

        private Coroutine _process;

        private void Activate()
        {
            _process = StartCoroutine(ActiveProcess());
        }
        public void DeActivate()
        {
            StopCoroutine(_process);
        }

        private IEnumerator ActiveProcess()
        {
            Color color = singMaterial.color;
            color.a = animatedMaterialWay.Evaluate(0);
            var time = 0.0f;
            while (true)
            {
                var op = time % cycleTime;
                color.a = animatedMaterialWay.Evaluate(op);
                
                singMaterial.color = color;
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
            }
        }

        protected override void OnAwake()
        {
            
        }

        protected override void OnStart()
        {
            Activate();
        }

        protected override void ReStart()
        {
        }

        protected override void Stop()
        {
        }
    }
}