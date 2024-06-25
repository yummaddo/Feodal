using System.Collections.Generic;

namespace Game.RepositoryEngine.Abstraction
{
    public interface ITemp<TEncoded, TEncodedIdentifier, TData>
    {
        public Dictionary<TEncoded, TData> Data { get; set; }
        public Dictionary<TEncodedIdentifier, TEncoded> EncodeByIdentifier { get; set; } 
        public Dictionary<TEncodedIdentifier, TData> DataByIdentifier { get; set; }
        public Dictionary<TEncodedIdentifier, int> ViewIndex { get; set; }
        public void Initialization(TEncoded encoded, TData amount);
    }
}