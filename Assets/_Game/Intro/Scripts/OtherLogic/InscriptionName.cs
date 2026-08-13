using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InscriptionName : MonoBehaviour
{
    [SerializeField] private float _stepScale;
    [SerializeField] private Vector2 _endScale;
    [SerializeField] private float _stepPosition;
    [SerializeField] private Vector2 _endPosition;

    private Vector2 _startScale;
    private Vector2 _currentScale;
    private Vector2 _startPosition;
    private Vector2 _currentPosition;
    private bool _increase;
    private RectTransform _rectTransform;
    private Shaking _shaking;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _shaking = GetComponent<Shaking>();
    }

    private void Start()
    {
        _startScale = transform.localScale;
        _currentScale = _startScale;

        _startPosition = _rectTransform.anchoredPosition;
        _currentPosition = _startPosition;
    }

    private void Update()
    {
        if (_increase)
            _currentScale = Vector2.Lerp(_currentScale, _endScale, _stepScale * Time.deltaTime);
        else
            _currentScale = Vector2.Lerp(_currentScale, _startScale, _stepScale * 50 * Time.deltaTime);

        if (_increase)
            _currentPosition = Vector2.Lerp(_currentPosition, _endPosition, _stepPosition * Time.deltaTime);
        else
            _currentPosition = Vector2.Lerp(_currentPosition, _startPosition, _stepPosition * 5 * Time.deltaTime);
        

        _rectTransform.localScale = _currentScale;
        //_shaking.StartPosition = transform.localPosition;
        _rectTransform.anchoredPosition = _currentPosition;
    }

    public void Increase(bool increase)
    {
        _increase = increase;
    }
}
