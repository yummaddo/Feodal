using Game.Core.Abstraction;

namespace Game.Core.Cells
{
    public class CellResourcePackaging
    {
        public IResource Resource;
        public int Value;
        public CellResourcePackaging(IResource resource, int value)
        {
            Resource = resource;
            Value = value;
        }
    }
}