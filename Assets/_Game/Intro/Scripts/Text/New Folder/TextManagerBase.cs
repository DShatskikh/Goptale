using UnityEngine;
using System.Collections.Generic;
using TMPro;

public abstract class TextManagerBase : MonoBehaviour
{
    [SerializeField] protected List<string> _texts = new List<string>();
    [SerializeField] protected float _speedRecruiting = 0.05f;
    [SerializeField] protected int _countSymbol = 1;
    [SerializeField] protected TextMeshProUGUI _textMeshPro;

    protected int _numberText = 0;
    protected int _lengthText = 0;

    private void Awake()
    {
        if (_texts.Count == 0) _texts.Add("не забудь добавить текст");
        _textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _numberText = 0;
        _lengthText = 0;
    }
}