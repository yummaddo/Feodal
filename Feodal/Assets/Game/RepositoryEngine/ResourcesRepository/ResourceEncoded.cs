using Game.DataStructures.Abstraction;

namespace Game.RepositoryEngine.ResourcesRepository
{

    [System.Serializable]
    public class ResourceEncoded
    {
        public string Title { get; set; }
        public ResourceEncoded(string title)
        {
            Title = title;
        }
        public ResourceEncoded(IResource resource)
        {
            Title = resource.Title;
        }
        public override string ToString()
        {
            return $"{Title}";
        }
    }
}