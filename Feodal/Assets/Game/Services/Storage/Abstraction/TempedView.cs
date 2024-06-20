namespace Game.Services.Storage.Abstraction
{
    [System.Serializable]
    public class TempedView<TEncodedIdentifier,TData>
    {
        public TEncodedIdentifier title;
        public TData value;
        public TempedView(TEncodedIdentifier title, TData value)
        {
            this.title = title;
            this.value = value;
        }
    }
}