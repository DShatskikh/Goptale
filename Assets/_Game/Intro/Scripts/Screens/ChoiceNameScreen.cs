using TMPro;
using UnityEngine;

namespace Screens
{
    public class ChoiceNameScreen : Screen
    {
        [SerializeField]
        private TMP_Text _name;
        
        [SerializeField]
        private TMP_Text[] _laters;

        [SerializeField]
        private TMP_Text[] _buttons;
        
        [SerializeField]
        private AcceptNameScreen _acceptNameScreen;
        
        private int _laterIndex;
        private int _buttonIndex;
        private bool _isLater = true;
        private bool _isButton;
        private bool _isInit;
        
        public override void Show()
        {
            base.Show();
            _name.gameObject.SetActive(true);
            _isInit = false;
        }

        private void Update()
        {
            if (!_canvasGroup.interactable)
                return;

            if (!_isInit)
            {
                _isInit = true;
                return;
            }
            
            foreach (var later in _laters)
            {
                later.color = Color.white;
            }
            
            foreach (var button in _buttons)
            {
                button.color = Color.white;
            }
            
            if (_isLater)
            {
                if (Input.GetButtonDown("Horizontal"))
                {
                    if (Input.GetAxis("Horizontal") > 0)
                    {
                        if (_laterIndex < 8)
                            _laterIndex++;
                        else if (_laterIndex > 8 && _laterIndex < 17)
                            _laterIndex++;
                        else if (_laterIndex > 17 && _laterIndex < 26)
                            _laterIndex++;
                        else if (_laterIndex > 26 && _laterIndex < 32)
                            _laterIndex++;
                    }
                    else if (Input.GetAxis("Horizontal") < 0)
                    {
                        if (_laterIndex > 0 && _laterIndex < 9)
                            _laterIndex--;
                        else if (_laterIndex > 9 && _laterIndex < 18)
                            _laterIndex--;
                        else if (_laterIndex > 18 && _laterIndex < 27)
                            _laterIndex--;
                        else if (_laterIndex > 27 && _laterIndex < 33)
                            _laterIndex--;
                    }
                }
                else if (Input.GetButtonDown("Vertical"))
                {
                    if (Input.GetAxis("Vertical") > 0)
                    {
                        if (_laterIndex > 8)
                            _laterIndex -= 9;
                    }
                    else if (Input.GetAxis("Vertical") < 0)
                    {
                        if (_laterIndex < 24)
                            _laterIndex += 9;
                        else if (_laterIndex >= 24)
                        {
                            _isButton = true;
                            _isLater = false;
                        }
                    }
                }
                else if (Input.GetButtonDown("Submit"))
                {
                    if (_name.text.Length < 6)
                        _name.text += _laters[_laterIndex].text;

                    foreach (var later in _laters)
                    {
                        later.text = later.text.ToLower();
                    }
                }

                _laters[_laterIndex].color = Color.yellow;
            }
            else if (_isButton)
            {
                if (Input.GetButtonDown("Horizontal"))
                {
                    if (Input.GetAxis("Horizontal") > 0)
                    {
                        if (_buttonIndex < 2)
                            _buttonIndex++;
                    }
                    else if (Input.GetAxis("Horizontal") < 0)
                    {
                        if (_buttonIndex > 0)
                            _buttonIndex--;
                    }
                }
                else if (Input.GetButtonDown("Vertical"))
                {
                    if (Input.GetAxis("Vertical") > 0)
                    {
                        _isButton = false;
                        _isLater = true;
                    }
                }
                else if (Input.GetButtonDown("Submit"))
                {
                    if (_buttonIndex == 0)
                    {
                        Application.Quit();
                    }
                    else if (_buttonIndex == 1)
                    {
                        if (_name.text.Length != 0)
                            _name.text = _name.text.Substring(0, _name.text.Length - 1);
                        
                        if (_name.text.Length == 0)
                        {
                            foreach (var later in _laters)
                            {
                                later.text = later.text.ToUpper();
                            }
                        }
                    }
                    else if (_buttonIndex == 2)
                    {
                        Close();
                        _acceptNameScreen.Show();
                        Stats.Instance.Name = _name.text;
                    }
                }

                _buttons[_buttonIndex].color = Color.yellow;
            }

            if (Input.GetButtonDown("Cancel"))
            {
                if (_name.text.Length != 0)
                    _name.text = _name.text.Substring(0, _name.text.Length - 1);

                if (_name.text.Length == 0)
                {
                    foreach (var later in _laters)
                    {
                        later.text = later.text.ToUpper();
                    }
                }
            }
        }
    }
}
