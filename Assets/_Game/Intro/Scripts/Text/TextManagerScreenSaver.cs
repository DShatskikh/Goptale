using System;
using System.Collections;
using Screens;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Text
{
    public class TextManagerScreenSaver : TextManagerBase
    {
        [SerializeField]
        private AudioClip[] _audioClips;
        
        [SerializeField]
        private LogoScreen _logoScreen;
        
        [SerializeField]
        private AudioClip _menuTheme;

        [SerializeField]
        private Image _blackScreen;
        
        private AudioSource _audioSource;
        private bool _isPause;
        
        public Action EndScreenSaver;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = _audioClips[0];
            StartCoroutine(CustonSpeedText());
        }

        private void Update()
        {
            if (_isPause)
                return;
            
            if (InputManager.Instance.IsSubmitDown)
            {
                _isPause = true;
                GetComponent<Animator>().enabled = false;

                StartCoroutine(AwaitClose());
            }
        }

        private IEnumerator AwaitClose()
        {
            var alpha = 0f;
            _blackScreen.gameObject.SetActive(true);

            while (alpha < 1)
            {
                _blackScreen.color = new Color(0f, 0f, 0f, alpha);
                alpha += Time.deltaTime;
                yield return null;
            }
            
            if (SaveSystem.IsSave())
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
            else
            {
                _blackScreen.gameObject.SetActive(false);
                CloseWindow();
                _logoScreen.Show();
            }
        }
        
        private void CloseWindow()
        {
            EndScreenSaver?.Invoke();
            gameObject.SetActive(false);
            MusicManager.Instance.Play(_menuTheme);
        }

        private void CloseText()
        {
            _textMeshPro.gameObject.SetActive(false);
        }

        private IEnumerator CustonSpeedText()
        {
            _audioSource.Play();
            _lengthText = 0;

            _textMeshPro.text = _texts[_numberText];

            while (_lengthText <= _texts[_numberText].Length)
            {
                _textMeshPro.maxVisibleCharacters = _lengthText;

                if (_lengthText < _texts[_numberText].Length)
                {
                    if (_lengthText % 5 == 0 || _texts[_numberText][_lengthText] == '.')
                    {
                        if (_lengthText % 2 == 0)
                        {
                            _audioSource.clip = _audioClips[1];
                        }
                        else if (_lengthText % 3 == 0)
                        {
                            _audioSource.clip = _audioClips[2];
                        }
                        else
                        {
                            _audioSource.clip = _audioClips[3];
                        }

                        _audioSource.Play();
                    }

                }

                _lengthText += _countSymbol;

                switch (_numberText)
                {
                    case 0:
                        if (_lengthText == 13 || _lengthText == 42) _speedRecruiting = 0.6f;
                        break;

                    case 1:
                        if (_lengthText == 51) _speedRecruiting = 0.6f;
                        break;

                    case 2:
                        if (_lengthText == 50) _speedRecruiting = 0.6f;
                        break;

                    case 3:
                        _speedRecruiting = 0.08f;
                        if (_lengthText == 17 || _lengthText == 18 || _lengthText == 19) _speedRecruiting = 0.6f;
                        if (_lengthText == 20) _audioSource.Play();
                        break;

                    case 4:
                        _speedRecruiting = 0.08f;
                        _textMeshPro.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 143.3816f);
                        break;

                    case 5:
                        _textMeshPro.GetComponent<RectTransform>().sizeDelta = new Vector2(900, 143.3816f);
                        break;

                    default:
                        break;
                }

                yield return new WaitForSeconds(_speedRecruiting);
                _speedRecruiting = 0.07f;
            }

            _numberText++;
        }
    }
}