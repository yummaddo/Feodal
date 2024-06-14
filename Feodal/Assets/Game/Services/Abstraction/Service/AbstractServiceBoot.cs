using System.Collections;
using Game.Meta;
using UnityEngine;

namespace Game.Services.Abstraction.Service
{
    public abstract class AbstractServiceBoot : MonoBehaviour
    {
        protected abstract bool Active();
        
        internal virtual bool ServiceActive(SessionActivityStates[] expect, SessionStateManager manager)
        {
            return ((IList)expect).Contains(manager.CurrentActivityState);
        }
        protected abstract void OnAwake();
        protected abstract void OnStart();
        
        internal IEnumerator BreakPointFixed()
        {
            if (!Active())
            {
                yield return new WaitUntil(Active);
            }
        }
        internal IEnumerator BreakPoint()
        {
            if (!Active())
            {
                yield return new WaitUntil(Active);
            }
        }
    }
}