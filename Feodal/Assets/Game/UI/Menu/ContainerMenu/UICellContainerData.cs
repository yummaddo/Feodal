using Game.Core.Abstraction.UI;
using Game.Core.DataStructures.UI.Data;

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