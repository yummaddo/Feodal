using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Animation
{
    public class UICellFarmerProgressBar : MonoBehaviour
    {
        [SerializeField]private Image image;
        [SerializeField]private Image resource;
        [SerializeField] private Text valueText;
        [SerializeField] private UICellPickUpAnimation animator;
        public Gradient gradient;

        public void Init(Sprite resourceImg)
        {
            resource.sprite = resourceImg;
        }
        public void GetResource(float value)
        {
            animator.Play(value);
        }
        public void ResourceNone()
        {
            animator.PlayNone();
        }
        public void SetFarmValue(float value)
        {
            valueText.text = value.ToString();
        }
        public void SetFarmFrame(float value)
        {
            if (value > 1.0f); value = Math.Clamp(value, 0,1);
            if (image) image.fillAmount = value;
            image.color = gradient.Evaluate(value);
        }
    }
}