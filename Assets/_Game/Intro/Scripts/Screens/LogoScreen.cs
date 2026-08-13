using System;
using System.Collections;
using System.Collections.Generic;
using Text;
using UnityEngine;

namespace Screens
{
    public class LogoScreen : Screen
    {
        private readonly string Submit = "Submit";
        
        [SerializeField] private TextManagerScreenSaver _textManagerScreenSaver;
        [SerializeField] private ControlScreen _controlScreen;
        [SerializeField] private GameObject _hint;

        private bool _isActive;
        
        private void Awake() => 
            _textManagerScreenSaver.EndScreenSaver += Show;

        private void Start()
        {
            StartCoroutine(HintOn());
        }

        private void Update()
        {
            if (Input.GetButtonDown(Submit) && _canvasGroup.alpha == 1 && _isActive)
            {
                _controlScreen.Show();
                Close();
            }
        }

        private IEnumerator HintOn()
        {
            yield return new WaitForSeconds(1);
            _hint.SetActive(true);
            _isActive = true;
        }
    }
}