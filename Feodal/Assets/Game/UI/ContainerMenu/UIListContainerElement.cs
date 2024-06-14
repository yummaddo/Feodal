using Game.Core.Abstraction.UI;
using Game.Core.DataStructures.UI;
using Game.Core.DataStructures.UI.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Game.UI.ContainerMenu
{

    public class UIListContainerElement : FancyCell<UICellContainerData, Context>
    {
        public UICellContainer uIContainer;

        public Animator animator;
        public AnimationCurve x;
        public Image cell;
        public Text title;
        public Image seed;
        public Text price;
        
        private static readonly int Scroll = Animator.StringToHash("scroll");
        private float _currentPosition = 0;
        internal IUICellContainer UIContainer;

        public override void UpdateContent(UICellContainerData itemContainerData)
        {
            UIContainer = itemContainerData.Data;
            cell.sprite = UIContainer.CellImage;
            seed.sprite = UIContainer.CellLendIdentImage;
            title.text = UIContainer.Container.containerName;
            price.text = UIContainer.Container.price.ToString();
        }

        public override void UpdatePosition(float position)
        {
            _currentPosition = position;
            if (animator.isActiveAndEnabled)
            {
                animator.Play(Scroll, -1, position);
            }
            animator.speed = 0;
        }
        void OnEnable() => UpdatePosition(_currentPosition);
    }
}