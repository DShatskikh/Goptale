using UnityEngine;

public sealed class GenocideThemePlay : MonoBehaviour
{
    [SerializeField]
    private AudioClip _normal, _genocide;
    
    private void Start()
    {
        MusicManager.Instance.Play(Stats.Instance.IsGenocide ? _genocide : _normal);
    }
}
