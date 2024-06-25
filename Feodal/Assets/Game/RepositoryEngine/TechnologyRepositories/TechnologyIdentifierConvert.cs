using Game.RepositoryEngine.Abstraction;

namespace Game.RepositoryEngine.TechnologyRepositories
{
    public class TechnologyIdentifierConvert: IIdentifier<string,TechnologyEncoded>
    {
        public string GetEncodedIdentifier(TechnologyEncoded amount)
        {
            return amount.title;
        }
    }
}