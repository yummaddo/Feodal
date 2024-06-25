using System;
using System.Collections;
using System.Threading.Tasks;
using Game.Services.Abstraction;
using UnityEngine;

namespace Game.Services.CellServices.Microservice
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
            while (gameObject.activeSelf)
            {
                var op = time % cycleTime;
                color.a = animatedMaterialWay.Evaluate(op);
                
                singMaterial.color = color;
                yield return new WaitForFixedUpdate();
                time += Time.fixedDeltaTime;
            }
        }
        protected override Task OnAwake(IProgress<float> progress) { return Task.CompletedTask; }
        protected override Task OnStart(IProgress<float> progress) { Activate();  return Task.CompletedTask;}
    }
}