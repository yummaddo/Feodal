using System;
using System.Globalization;
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
            animator = GetComponent<UICellPickUpAnimation>();
            animator.Initialization();
        }
        public void GetResource(float value)
        {
            try
            {
                animator.Play(value);

            }
            catch (Exception e)
            {
                // ignored
            }
        }
        public void ResourceNone()
        {
        }
        public void SetFarmValue(float value)
        {
            try
            {
                valueText.text = value.ToString(CultureInfo.CurrentCulture);
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        public void SetFarmFrame(float value)
        {
            if (value > 1.0f); value = Math.Clamp(value, 0,1);
            if (image) image.fillAmount = value;
            image.color = gradient.Evaluate(value);
        }
    }
}