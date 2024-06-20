using Game.Core.DataStructures.Technologies.Abstraction;

namespace Game.Services.Storage.TechnologyRepositories
{
    [System.Serializable]
    public class TechnologyCoded
    {
        public string encodedTitle;
        public TechnologyCoded(string encodedTitle)
        {
            this.encodedTitle = encodedTitle;
        }
        public TechnologyCoded(ITechnology encodedTitle)
        {
            this.encodedTitle = encodedTitle.Title;
        }
    }
}