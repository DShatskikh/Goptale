using System;
using UnityEngine;

public sealed class GenocideThemePlay : MonoBehaviour
{
    private static float _genocideTime;
    
    [SerializeField]
    private AudioClip _normal, _genocide;
    
    private void Start()
    {
        MusicManager.Instance.Play(Stats.Instance.IsGenocide ? _genocide : _normal);

        if (Stats.Instance.IsGenocide)
        {
            MusicManager.Instance.SetTime(_genocideTime);
        }
    }

    private void OnDestroy()
    {
        if (Stats.Instance.IsGenocide)
            _genocideTime = MusicManager.Instance.GetTime;
    }
}
