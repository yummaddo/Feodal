using System;

namespace Game.Core.DataStructures.UI
{
    public class Context
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellClicked;
    }
}