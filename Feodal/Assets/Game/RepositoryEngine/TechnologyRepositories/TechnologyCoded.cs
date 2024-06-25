using Game.DataStructures.Technologies.Abstraction;

namespace Game.RepositoryEngine.TechnologyRepositories
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
        public override string ToString()
        {
            return $"{encodedTitle}";
        }
    }
}