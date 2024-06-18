using System.Collections.Generic;
using Game.Core.Abstraction;

namespace Game.Core.DataStructures.Technology
{
    public class TradeTechnology: AbstractTechnology<TradeTechnology>
    {
        protected override TradeTechnology CompareTemplate()
        {
            throw new System.NotImplementedException();
        }

        protected override string DataNamePattern => "";

        public override void Trade()
        {
            throw new System.NotImplementedException();
        }

        public override bool Status()
        {
            throw new System.NotImplementedException();
        }

        internal override void Initialization()
        {
            throw new System.NotImplementedException();
        }
    }
}