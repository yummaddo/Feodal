using System;

namespace Game.DataStructures.UI
{
    public class Context
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellClicked;
    }
}