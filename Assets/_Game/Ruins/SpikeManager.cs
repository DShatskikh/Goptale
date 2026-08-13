using UnityEngine;

public sealed class SpikeManager : MonoBehaviour
{
    [SerializeField]
    private int _index;
    
    [SerializeField]
    private Lenin[] _lenins;

    [SerializeField]
    private GameObject[] _spikes;
    
    [SerializeField]
    private Sprite _deactivated;

    [SerializeField]
    private Sprite _activated;
    
    [SerializeField]
    private AudioSource _sfx;
    
    private bool _isActivate;
    public bool IsActivate => _isActivate;

    private void Start()
    {
        // if (Stats.Instance.IsGenocide)
        // {
        //     Stats.Instance.SpikePuzzle[_index] = true;
        // }
        
        if (Stats.Instance.SpikePuzzle[_index])
        {
            foreach (var spike in _spikes)
            {
                spike.GetComponent<Collider2D>().enabled = false;
                spike.GetComponentsInChildren<SpriteRenderer>()[1].sprite = _deactivated;
            }
            
            foreach (var lenin in _lenins)
            {
                lenin.IsActivate = true;
            }
            
            _isActivate = true;
        }
    }

    private void Update()
    {
        if (_isActivate)
            return;

        var isActivate = true;
        
        foreach (var lenin in _lenins)
        {
            if (!lenin.IsActivate)
                isActivate = false;
        }
        
        _isActivate = isActivate;

        if (_isActivate)
        {
            foreach (var spike in _spikes)
            {
                spike.GetComponent<Collider2D>().enabled = false;
                spike.GetComponentsInChildren<SpriteRenderer>()[1].sprite = _deactivated;
            }
            
            _sfx.Play();

            Stats.Instance.SpikePuzzle[_index] = true;
        }
    }

    public void Activate()
    {
        _isActivate = false;
        
        foreach (var spike in _spikes)
        {
            spike.GetComponent<Collider2D>().enabled = true;
            spike.GetComponentsInChildren<SpriteRenderer>()[1].sprite = _activated;
        }
    }
}
