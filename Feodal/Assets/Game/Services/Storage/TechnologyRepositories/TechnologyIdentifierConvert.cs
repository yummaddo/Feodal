using Game.Services.Storage.Abstraction;

namespace Game.Services.Storage.TechnologyRepositories
{
    public class TechnologyIdentifierConvert: IIdentifier<string,TechnologyEncoded>
    {
        public string GetEncodedIdentifier(TechnologyEncoded amount)
        {
            return amount.title;
        }
    }
}