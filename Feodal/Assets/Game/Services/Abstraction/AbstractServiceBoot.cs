using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Services.Abstraction
{
    public abstract class AbstractServiceBoot : MonoBehaviour
    {
        protected abstract Task OnAwake(IProgress<float> progress);
        protected abstract Task OnStart(IProgress<float> progress);
    }
}