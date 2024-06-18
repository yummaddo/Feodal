namespace Game.Services.Storage.ResourcesRepository
{
    [System.Serializable]
    public class ResourceCoded
    {
        public string encryptedTitle;
        public string encryptedType;
        public string encryptedRare;
        public string encryptedQuantity;
        public ResourceCoded(string encryptedTitle, string encryptedType, string encryptedRare, string encryptedQuantity)
        {
            this.encryptedTitle = encryptedTitle;
            this.encryptedType = encryptedType;
            this.encryptedRare = encryptedRare;
            this.encryptedQuantity = encryptedQuantity;
        }
        public override string ToString()
        {
            return $"{encryptedTitle}_{encryptedType}_{encryptedRare},{encryptedQuantity}";
        }
    }
}