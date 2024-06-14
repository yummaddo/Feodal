using Game.Core.Abstraction.UI;

namespace Game.UI.BuildingCellMenuScroll
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