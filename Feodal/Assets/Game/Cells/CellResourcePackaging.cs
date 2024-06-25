using Game.DataStructures.Abstraction;

namespace Game.Cells
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