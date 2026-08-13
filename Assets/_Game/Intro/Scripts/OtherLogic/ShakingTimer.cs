using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingTimer : MonoBehaviour
{
    [SerializeField] private float _delay = 0.04f;
    [SerializeField] private List<Shaking> _shakings = new List<Shaking>();

    private float _currentTime;

    private void Awake()
    {
        _currentTime = _delay;
    }
    
    private void Update()
    {
        Countdown();
    }

    private void Countdown()
    {
        if (_currentTime <= 0)
        {
            foreach (var shaking in _shakings) 
                shaking.Shake();

            _currentTime = _delay;
        }

        _currentTime -= Time.deltaTime;
    }
}
