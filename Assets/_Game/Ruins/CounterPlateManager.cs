using System;
using UnityEngine;

public sealed class CounterPlateManager : MonoBehaviour
{
    [SerializeField]
    private int _id;
    
    [SerializeField]
    private GameObject[] _spikes;
    
    [SerializeField]
    private CounterPlate[] _plates;

    [SerializeField]
    private AudioSource _sfx;
       
    [SerializeField]
    private Sprite _deactivated;

    [SerializeField]
    private int[] _answer = new int[4];

    private bool IsActive = true;

    private void Start()
    {
        if (Stats.Instance.PlatePuzzle[_id])
        {
            IsActive = false;
            
            foreach (var spike in _spikes)
            {
                spike.GetComponent<Collider2D>().enabled = false;
                spike.GetComponentsInChildren<SpriteRenderer>()[1].sprite = _deactivated;
            }

            for (var i = 0; i < _plates.Length; i++)
            {
                var plate = _plates[i];
                plate.IsActive = false;
                plate.Counter = _answer[i];
                plate.Upgrade();
            }
        }
    }

    private void Update()
    {
        if (!IsActive)
            return;
        
        var isEnd = true;
        
        for (int i = 0; i < _plates.Length; i++)
        {
            if (_plates[i].Counter != _answer[i])
            {
                isEnd = false;
            }
        }

        if (isEnd)
        {
            foreach (var spike in _spikes)
            {
                spike.GetComponent<Collider2D>().enabled = false;
                spike.GetComponentsInChildren<SpriteRenderer>()[1].sprite = _deactivated;
            }

            foreach (var plate in _plates)
            {
                plate.IsActive = false;
            }
                
            IsActive = false;
            _sfx.Play();
            Stats.Instance.PlatePuzzle[_id] = true;
        }
    }
}
