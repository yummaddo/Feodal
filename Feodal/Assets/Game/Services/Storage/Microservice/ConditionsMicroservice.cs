using System.Collections.Generic;
using Game.Core.DataStructures.Conditions;
using Game.Services.Abstraction.MicroService;

namespace Game.Services.Storage.Microservice
{
    public class ConditionsMicroservice : AbstractMicroservice<StorageService>
    {
        protected override void OnAwake() { }
        protected override void OnStart() { }
        protected override void ReStart() { }
        protected override void Stop() { }
    }
}