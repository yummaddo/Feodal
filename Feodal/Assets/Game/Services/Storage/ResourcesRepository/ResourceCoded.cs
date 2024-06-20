namespace Game.Services.Storage.ResourcesRepository
{
    [System.Serializable]
    public class ResourceCoded
    {
        public string encryptedTitle;
        public ResourceCoded(string encryptedTitle)
        {
            this.encryptedTitle = encryptedTitle;
        }
        public override string ToString()
        {
            return $"{encryptedTitle}";
        }
    }
}