using System;
using System.Collections.Generic;
using Game.CallBacks;
using Game.DataStructures.Technologies;
using Game.DataStructures.Technologies.Abstraction;
using Game.RepositoryEngine.Abstraction;
using Game.RepositoryEngine.ResourcesRepository;
using Game.Services.ProxyServices;
using Game.Services.ProxyServices.Abstraction;
using Game.Utility;

namespace Game.RepositoryEngine.TechnologyRepositories
{
    public class TechnologyTemp : Temp<TechnologyEncoded, string, bool>, ICallBack<TechnologyTempedCallBack>
    {
        internal Dictionary<string, ITechnologyStore> TechnologyStores;
        internal Dictionary<string, ITradeBuildTechnology> TechnologyBuild;
        public bool IsInit { get; set; }
        public Action<Port, TechnologyTempedCallBack> OnCallBackInvocation { get; set; } = (port, callback) =>
        {
            Debugger.Logger($"[{port}]=>callback:{callback.Technology.Title}=>{callback.Value}", ContextDebug.Session, Process.Update);
        };
        internal void InjectTechnologies(List<Technology> technologies, TechnologyRepository repository)
        {
            TechnologyStores = new Dictionary<string, ITechnologyStore>();
            foreach (var rTechnology in technologies)
            {
                var temped = rTechnology.Data; 
                temped.Temp = this;
                temped.Repository = repository;
                TechnologyStores.Add(temped.Title, temped);
            }
        }
        internal void InjectTechnologies(List<TradeBuildTechnology> technologies, TechnologyRepository repository, ResourceTemp resourcesRepository)
        {
            TechnologyBuild = new Dictionary<string, ITradeBuildTechnology>();
            foreach (var rTechnology in technologies)
            {
                var temped = rTechnology.Data; 
                temped.Temp = this;
                temped.ResourceTemp = resourcesRepository;
                temped.Repository = repository;
                TechnologyBuild.Add(temped.Title, temped);
            }
        }
        internal override void Injection()
        {
            base.Injection();
            OnEncodeChangeData = EncodeChangeElement;
            OnEncodeChangeElement = EncodeChange;
            
        }
        private void EncodeChange(string arg1, TechnologyEncoded resourceEncoded)
        {
        }
        private void EncodeChangeElement(string arg1, bool arg2)
        {
            Debugger.Logger("TechnologyTemp EncodeChangeElement");
            var title = arg1;
            var resource = TechnologyStores[title];
            OnCallBackInvocation?.Invoke(Porting.Type<TechnologyTempedCallBack>(), new TechnologyTempedCallBack(title, resource, arg2));
            
        }
        protected internal TechnologyTemp(IIdentifier<string, TechnologyEncoded> identifier) : base(identifier)
        {
            TechnologyStores = new Dictionary<string, ITechnologyStore>();
            TechnologyBuild = new Dictionary<string, ITradeBuildTechnology>();
        }
    }
}