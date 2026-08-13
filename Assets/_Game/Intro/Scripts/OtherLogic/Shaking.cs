using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shaking : MonoBehaviour
{
    [SerializeField] private float _distance = 1.5f;
    [SerializeField] private float _speed = 10f;

    private Transform _transform;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    private float _currentTime;
    private bool _isShaking;
    private bool _isInit;

    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        yield return null;
        _transform = transform;
        _startPosition = _transform.localPosition;
        _isInit = true;
    }

    private void Update()
    {
        if (!_isInit)
            return;
        
        if (_isShaking)
        {
            Countdown();
        }
    }

    public void Shake()
    {
        if (!_isInit || !_transform)
            return;
        
        _targetPosition = _startPosition + new Vector2(
            Random.Range(-_distance, _distance),
            Random.Range(-_distance, _distance)
        );
        
        _currentTime = 0;
        _isShaking = true;
    }

    private void MoveToShake()
    {
        _transform.localPosition = Vector2.Lerp(_startPosition, _targetPosition, _currentTime);
    }

    private void Countdown()
    {
        if (_currentTime <= 1)
        {
            MoveToShake();
            _currentTime += Time.deltaTime * _speed;
        }
        else
        {
            _transform.localPosition = _startPosition;
            _isShaking = false;
        }
    }
}
