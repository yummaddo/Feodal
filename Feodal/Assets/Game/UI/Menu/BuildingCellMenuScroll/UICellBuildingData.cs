using Game.Core.Abstraction.UI;

namespace Game.UI.Menu.BuildingCellMenuScroll
{
    public class UICellBuildingData
    {
        public UICellBuildingData(IUICellContainerElement data)
            {
                this.Data = data;
            }
        internal IUICellContainerElement Data { get; }
    }
}