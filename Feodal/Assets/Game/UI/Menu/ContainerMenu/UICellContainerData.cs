using Game.DataStructures.UI;

namespace Game.UI.Menu.ContainerMenu
{
    public class UICellContainerData
    {
        public UICellContainerData(UICellContainer data)
        {
            this.Data = data;
        }
        internal UICellContainer Data { get; }
    }
}