namespace Game.Services.Storage.Abstraction
{
    public interface IIdentifier< out TIdentifier, in TData>
    {
        public TIdentifier GetEncodedIdentifier(TData amount);
    }
}