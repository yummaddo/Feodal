using Game.DataStructures.Technologies.Abstraction;

namespace Game.RepositoryEngine.TechnologyRepositories
{
    [System.Serializable]
    public class TechnologyEncoded
    {
        public string title;
        public TechnologyEncoded(string codedTitle)
        {
            this.title = codedTitle;
        }
        public TechnologyEncoded(ITechnology codedTitle)
        {
            this.title = codedTitle.Title;
        }
        public override string ToString()
        {
            return $"{title}";
        }
    }
}