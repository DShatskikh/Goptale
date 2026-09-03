using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Screens
{
    public class AcceptNameScreen : Screen
    {
        [SerializeField]
        private Image _image;
        
        [SerializeField]
        private float _stepColor;
        
        [SerializeField]
        private Color32 _endColor;
        
        [SerializeField]
        private SceneSwitch _sceneSwitch;

        [SerializeField]
        private TMP_Text[] _buttons;

        [SerializeField]
        private ChoiceNameScreen _choiceNameScreen;

        [SerializeField]
        private InscriptionName _inscriptionName;
        
        [SerializeField]
        private TMP_Text _info;
        
        [SerializeField]
        private AudioSource _sfx;
        
        private int _index = 0;
        private bool _isInit;
        private float _startAlpha;
        private float _currentAlpha;
        private bool _trigger;
        private bool _isEnd;
        private bool _canRight = true;

        private void Awake()
        {
            _startAlpha = 0;
            _currentAlpha = _startAlpha;
        }

        public override void Show()
        {
            _inscriptionName.Increase(true);
            base.Show();

            var text = _inscriptionName.GetComponent<TMP_Text>().text.ToUpper();
            var isSimilar = true;

            foreach (var symbol in text)
            {
                if (symbol != text[0])
                    isSimilar = false;
            }

            _canRight = true;
            
            if (isSimilar)
            {
                _info.text = "Очень оригинально";
            }
            else if (text == "КРИС")
            {
                _info.text = "Та самая?";
            }
            else if (text == "ДЕКАРД" || text == "ДЕКАРТ")
            {
                _info.text = "О легенда!";
            }
            else if (text == "ФРОСТ")
            {
                _info.text = "Это спарта!";
            }
            else if (text == "СОМ")
            {
                _info.text = "Заебала эта ебатория.";
            }
            else if (text == "ПЕТУХ")
            {
                _info.text = "Здесь таких не уважают.";
            }
            else if (text == "ЧИРИК" || text == "ТОМАРА" || text == "ТОМА")
            {
                _info.text = "Это имя уже занято!";
                _canRight = false;
            }
            else if (text == "САНС" || text == "ЧАРА")
            {
                _info.text = "Браток, это не та игра.";
                _canRight = false;
            }
            else if (text == "ЕБЛАН" || text == "ХУЙ" || text == "ЧЛЕН" || text == "ПИСЬКА" || text == "ПИПКА" || text == "ЛОХ")
            {
                _info.text = "Пиздец.";
            }
            else if (text == "ГОПНИК")
            {
                _info.text = "Опять гопота.";
            }
            else if (text == "НАРИК")
            {
                _info.text = "Наркоманы!";
            }
            else if (text == "АЛКАШ")
            {
                _info.text = "Алкаши!";
            }
            else if (text == "ЧАРИК")
            {
                _info.text = "Неа.";
            }
            else if (text == "ГАСТЕР")
            {
                _info.text = "???";
                _canRight = false;
            }
            else if (text == "НЕДЕНИС" || text == "НЕДЕНИ")
            {
                _info.text = "???";
            }
            else if (text == "ФЛРЕЗИК") // flrezik
            {
                _info.text = "О легенда!";
            }
            else if (text == "ВАЛЕРА")
            {
                _info.text = "Настало твоё время!";
            }
            else if (text == "ПУТИН")
            {
                _info.text = "Владимир Владимирович?";
            }
            else if (text == "СТАЛКЕР")
            {
                _info.text = "Иди своей дорогой.";
            }
            else
            {
                _info.text = "Выбрать это имя?";
            }

            _buttons[1].gameObject.SetActive(_canRight);
        }

        public override void Close()
        {
            _inscriptionName.Increase(false);
            base.Close();
        }

        private void Update()
        {
            ChangeAlpha();
            
            if (!_canvasGroup.interactable)
                return;

            if (!_isInit)
            {
                _isInit = true;
                return;
            }
            
            foreach (var button in _buttons)
            {
                button.color = Color.white;
            }
            
            if (_isEnd)
                return;

            if (!_canRight)
                _index = 0;
            
            if (InputManager.Instance.IsHorizontalDown)
            {
                if (InputManager.Instance.Horizontal > 0)
                {
                    if (_index == 0 && _canRight)
                        _index = 1;
                }
                else if (InputManager.Instance.Horizontal < 0)
                {
                    if (_index == 1)
                        _index = 0;
                }
            }
            else if (InputManager.Instance.IsSubmitDown)
            {
                if (_index == 0)
                {
                    Close();
                    _choiceNameScreen.Show();   
                }
                else if (_index == 1)
                {
                    _canvasGroup.interactable = false;
                    ActivateTrigger();
                    _isEnd = true;
                }
            }
            
            _buttons[_index].color = Color.yellow;
        }
        
        public void ActivateTrigger() => 
            _trigger = true;

        private void ChangeAlpha()
        {
            if (_trigger)
            {
                if (_currentAlpha < 1)
                    _currentAlpha += _stepColor * Time.deltaTime / 255;
                else
                {
                    _trigger = false;
                    CoroutineRunner.Instance.StartCoroutine(StartScene());
                }
            }
            
            _image.color = new Color(1, 1, 1, _currentAlpha);
        }

        private IEnumerator StartScene()
        {
            MusicManager.Instance.Stop();
            _sfx.Play();
            yield return new WaitForSeconds(4f);

            yield return SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Single);
            var majorZvetkov = Instantiate(Resources.Load<GameObject>("Major Zvetcov"));
            SceneManager.MoveGameObjectToScene(majorZvetkov, SceneManager.GetSceneByName("Battle"));
            Fedya.IsLoad = true;
        }
    }
}
