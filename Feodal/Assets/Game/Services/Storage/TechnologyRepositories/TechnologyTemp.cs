using System.Collections.Generic;
using Game.Core.DataStructures.Technologies;
using Game.Core.DataStructures.Technologies.Abstraction;
using Game.Core.DataStructures.Technologies.Base;
using Game.Services.Storage.Abstraction;
using Game.Services.Storage.ResourcesRepository;

namespace Game.Services.Storage.TechnologyRepositories
{
    public class TechnologyTemp : Temp<TechnologyEncoded, string, bool>
    {
        internal Dictionary<string, ITechnologyStore> TechnologyStores;
        internal Dictionary<string, ITradeBuildTechnology> TechnologyBuild;
        internal void InjectTechnologies(List<Technology> technologies, TechnologyRepository repository)
        {
            TechnologyStores = new Dictionary<string, ITechnologyStore>();
            foreach (var rTechnology in technologies)
            {
                var temped = rTechnology.Data; 
                temped.Temp = this;
                temped.Repository = repository;
                TechnologyStores.Add(rTechnology.Data.Title, temped);
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
                TechnologyBuild.Add(rTechnology.Data.Title, temped);
            }
        }
        internal override void Injection()
        {
            base.Injection();
        }
        protected internal TechnologyTemp(IIdentifier<string, TechnologyEncoded> identifier) : base(identifier)
        {
            TechnologyStores = new Dictionary<string, ITechnologyStore>();
            TechnologyBuild = new Dictionary<string, ITradeBuildTechnology>();
        }
    }
}