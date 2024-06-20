namespace Game.Core.DataStructures
{
    [System.Serializable]
    public class ResourceCounter
    {
        public Resource resource;
        public int value;

        public ResourceCounter(Resource resource, int value)
        {
            this.resource = resource;
            this.value = value;
        }
    }
}