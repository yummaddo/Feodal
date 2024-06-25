using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class Boot : MonoBehaviour
    {
        private ApplicationContext _context;
        [SerializeField] private SessionLifeStyleManager lifeStyleManager;
        public Slider slider;
        public Gradient gradient;
        public Image fill;
        public Text text;
        [SerializeField] private GameObject loadBar;

        private void SetProgress(float health)
        {
            slider.value = health;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        private void Awake()
        {
            _context = ApplicationContext.Instance;
            lifeStyleManager.SessionAwake();
        }

        private void Start()
        {
            OnSessionInit();
        }

        private void OnSessionInit()
        {
            lifeStyleManager = SessionLifeStyleManager.Instance;
            lifeStyleManager.OnLoadProcess += (f, s) =>
            {
                SetProgress(f);
                text.text = s;
            };
            _context.OnLoadGameBoot?.Invoke(this);
        }

        public void OnComeBackToMainMenu()
        {
            _context.OnLoadStartMenu?.Invoke();
        }

        public async Task BootOperation()
        {
            await lifeStyleManager.AwakeStartOperation();
            loadBar.SetActive(false);
        }
    }
}