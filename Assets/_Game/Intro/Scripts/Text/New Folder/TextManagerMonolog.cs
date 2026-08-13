using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TextManagerMonolog : TextManagerBase
{
    [SerializeField] private Animator _animator;
    [SerializeField] private UnityEvent _event;
    [SerializeField] private UnityEvent _eventShowText;
    [SerializeField] private List<int> _feelings;

    private void Awake()
    {
        if (_texts.Count == 0) _texts.Add("не забудь добавить текст");

        if (_textMeshPro)
        {
            _textMeshPro = Instantiate<TextMeshProUGUI>(_textMeshPro, transform.position + _textMeshPro.transform.position, Quaternion.identity, transform);
        }
        else
        {
            _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        }

    }

    private void OnEnable()
    {
        _numberText = 0;
        _lengthText = 0;
        StartCoroutine(ShowText());
    }

    private void Start()
    {
        if (_animator) _animator = Instantiate<Animator>(_animator, transform.position + new Vector3(-278.1f, 7.1f, 0f), Quaternion.identity, transform);
        //if (GetComponent<Animator>()) _animator = GetComponent<Animator>();
        //StartCoroutine(ShowText());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel")) ShowAllText();
    }

    public void Initialized(List<string> texts)
    {
        _texts = texts;
    }

    private void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    public void ShowAllText()
    {
        _lengthText = _texts[_numberText].Length;
        _eventShowText.Invoke();
    }

    //public void SetEvents(UnityEvent unityEvent)
    //{
    //_event = unityEvent;
    //}

    public IEnumerator ShowText()
    {
        _lengthText = 0;
        _textMeshPro.text = _texts[_numberText];

        if (_animator) _animator.SetInteger("Feeling", 1);

        while (_lengthText <= _texts[_numberText].Length)
        {
            //_textMeshPro.text = _texts[_numberText].Substring(0, _lengthText);
            _textMeshPro.maxVisibleCharacters = _lengthText;
            _lengthText += _countSymbol;

            yield return new WaitForSeconds(_speedRecruiting);
        }

        if (_animator) _animator.SetInteger("Feeling", 0);

        if (_eventShowText.GetPersistentEventCount() != 0)
        {
            if (_numberText == _texts.Count - 1) ShowAllText();
        }
        else
        {
            yield return new WaitUntil(() => Input.GetButtonDown("Submit"));

            if (_numberText == _texts.Count - 1)
            {
                _event.Invoke();
                CloseWindow();
            }
            else
            {
                _numberText++;
                StartCoroutine(ShowText());
            }
        }
    }
}
