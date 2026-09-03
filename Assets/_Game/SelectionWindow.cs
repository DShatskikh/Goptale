using System.Collections;
using TMPro;
using UnityEngine;

public sealed class SelectionWindow : MonoBehaviour
{
    public static SelectionWindow Instance;
    public static AudioClip AudioClip;
    public static bool IsRight;

    [SerializeField]
    private TMP_Text _label, _leftLabel, _rightLabel;

    [SerializeField]
    private GameObject _determination;
    
    private string _text;
    private string _leftText;
    private string _rightText;
    private bool _isSkip;
    private bool _isEndAnimation;
    private float _duration = 0.05f;

    public static SelectionWindow StartDialogue(string text, string leftText, string rightText, bool isDown = false)
    {
        var dialogueWindow = Instantiate(Resources.Load<SelectionWindow>("Selection Window"),
            new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 3.884f), Camera.main.transform.rotation);
        
        dialogueWindow.SetText(text,leftText,rightText);
        Instance = dialogueWindow;
        IsRight = false;

        if (isDown)
        {
            Instance.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 3.884f);
        }
        
        return dialogueWindow;
    }
    
    private void Update()
    {
        if (!IsRight)
        {
            _determination.transform.localPosition = new Vector3(-5.96f, -0.98f);
        }
        else
        {
            _determination.transform.localPosition = new Vector3(0.39f, -0.98f);
        }

        if (InputManager.Instance.IsHorizontalDown)
        {
            if (!_determination.activeSelf)
                return;
            
            if (InputManager.Instance.Horizontal > 0)
            {
                IsRight = true;
            }
            else if (InputManager.Instance.Horizontal < 0)
            {
                IsRight = false;
            }
        }
        else if (InputManager.Instance.IsCancelDown)
        {
            _isSkip = true;
        } 
        else if (InputManager.Instance.IsSubmitDown)
        {
            
        }
    }
    
    public void SetText(string text, string leftText, string rightText)
    {
        _text = text;
        _label.text = string.Empty;
        _leftLabel.text = string.Empty;
        _rightLabel.text = string.Empty;

        _leftText = leftText;
        _rightText = rightText;
        
        StartCoroutine(AwaitWrite());
    }
    
    private IEnumerator AwaitWrite()
    {
        GetComponent<AudioSource>().clip = AudioClip;
        
        for (int i = 0; i < _text.Length; i++)
        {
            if (!_isSkip)
                yield return new WaitForSeconds(_duration);

            switch (_text[i])
            {
                case '\n':
                    _label.text += _text[i];
                    break;
                case '%':
                    if (!_isSkip)
                        yield return new WaitForSeconds(0.5f);
                    break;
                case '*':
                    _label = Instantiate(_label, transform);
                    _label.transform.position = new Vector3(_label.transform.position.x, 
                        _label.transform.position.y - 1f, _label.transform.position.z);
                    _label.text = string.Empty;
                    break;
                case '<':
                        if (_text[i + 1] == 'c' || _text[i + 2] == 'c')
                        {
                            while (_text[i] != '>')
                            {
                                _label.text += _text[i];
                                i++;
                            }
                        
                            _label.text += _text[i];
                            break;
                        }
                        
                        if (_text[i + 1] != '/')
                        {
                            for (; i < _text.Length; i++)
                            {
                                if (_text[i] == '>')
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (; i < _text.Length; i++)
                            {
                                if (_text[i] == '>')
                                {
                                    break;
                                }
                            }
                        }
                        
                        break;
                default:
                    _label.text += _text[i];
                    
                    if (!_isSkip && !GetComponent<AudioSource>().isPlaying)
                        GetComponent<AudioSource>().Play();
                    
                    break;
            }
        }

        for (int i = 0; i < _leftText.Length; i++)
        {
            if (!_isSkip)
                yield return new WaitForSeconds(_duration);
            
            _leftLabel.text += _leftText[i];
        }

        for (int i = 0; i < _rightText.Length; i++)
        {
            if (!_isSkip)
                yield return new WaitForSeconds(_duration);
            
            _rightLabel.text += _rightText[i];
        }
        
        _determination.SetActive(true);
        
        yield return new WaitUntil(() => InputManager.Instance.IsSubmitDown);
        Destroy(gameObject);
    }
}
