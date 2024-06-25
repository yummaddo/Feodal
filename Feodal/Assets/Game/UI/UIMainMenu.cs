using System;
using UnityEngine;

namespace Game.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        private ApplicationContext _context;
        private void Awake()
        {
            _context = ApplicationContext.Instance;
        }
        public void OnExit()
        {
            _context.OnExit?.Invoke();
        }
        public void OnReset()
        {
            _context.OnResetProgress?.Invoke();
        }
        public void OnLoadGame()
        {
            _context.OnLoadGame?.Invoke(this.gameObject);
        }
    }
}