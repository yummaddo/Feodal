using Game.Core.Abstraction;
using Game.Core.DataStructures;
using Game.Services.Proxies.ClickCallback.Abstraction;
using Game.Services.Proxies.Providers;
using UnityEngine;

namespace Game.Services.Proxies.ClickCallback.Button
{
    public class ButtonResourceCallBack : ButtonClickCallback<IResource>
    {
        public bool isUniversal = false; 
        [SerializeField] private Resource resource;
        public override void Initialization()
        {
            if (isUniversal)
            {
                UniversalResourceProvider.CallBackTunneling(this);
            }
            else
            {
                ResourceProvider.CallBackTunneling(this);
            }
            StatusInit = true;
            DataInitialization(resource.Data);
        }
    }
}