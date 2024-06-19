using Game.Core.DataStructures.Technologies.Abstraction;
using UnityEngine;

namespace Game.Core.DataStructures.Technologies.Base
{
    [CreateAssetMenu(menuName = "Technology/Cell")]
    public class CellTechnology :  AbstractDataStructure<ICellTechnology>, ICellTechnology
    {
        public bool CurrentStatus { get; set; }
        [field: SerializeField] public CellContainer Container { get; set; }
        [field: SerializeField] public string Title { get; set; }
        internal override string DataNamePattern => $"Technology_Trade_{Container.containerName}";

        protected override ICellTechnology CompareTemplate()
        {
            return this;
        }

        public bool Status()
        {
            throw new System.NotImplementedException();
        }
    }
}