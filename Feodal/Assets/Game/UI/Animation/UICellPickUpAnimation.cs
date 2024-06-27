using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Game.UI.Animation
{
    public class UICellPickUpAnimation : MonoBehaviour
    {
        public Animator animator;
        public GameObject animatorTarget;
        public Text Text;

        public void Initialization()
        {
            animator = animatorTarget.GetComponent<Animator>();
        }

        public void Play(float value)
        {
            Text.text = value.ToString();
            animator.Play("ResourcePick");
        }
    }
}