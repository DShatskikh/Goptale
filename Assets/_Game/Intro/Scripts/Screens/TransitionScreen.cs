using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    public class TransitionScreen : Screen
    {
        private const float WaitPaintingAlpha = 0.03f;
        
        [SerializeField] 
        private Image _background;

        private Color _color;
        private WaitForSeconds _waitForSeconds;
        public event Action OpenEnded;

        private void Awake()
        {
            _waitForSeconds = new WaitForSeconds(WaitPaintingAlpha);
        }

        public override void Show()
        {
            base.Show();
            StartCoroutine(OnOpen());
        }

        private IEnumerator OnOpen()
        {
            _color = new Color(1, 1, 1, 0);
            
            while (_color.a < 1)
            {
                _color.a += 0.01f;
                _background.color = _color;
                yield return _waitForSeconds;
            }
            
            OpenEnded?.Invoke();
        }
    }
}
