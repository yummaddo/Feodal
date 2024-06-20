using UnityEngine;

namespace Game.Core.Cells
{
    public class CellVisualSelection : MonoBehaviour
    {
        public Vector3 targetcale = new Vector3(1.1f,1.4f,1.2f);
        public Transform target;
        private Vector3 _baseScale = Vector3.one;
        public void Select()
        {
            // _baseScale = target.localScale;
            // target.localScale = targetcale;
        }
        public void UnSelect()
        {
            // target.localScale = _baseScale;
        }
    }
}
