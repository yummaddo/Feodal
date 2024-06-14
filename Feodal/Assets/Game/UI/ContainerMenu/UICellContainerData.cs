using Game.Core.Abstraction.UI;

namespace Game.UI.ContainerMenu
{
    public class UICellContainerData
    {
        public UICellContainerData(IUICellContainer data)
        {
            this.Data = data;
        }
        internal IUICellContainer Data { get; }
    }
}