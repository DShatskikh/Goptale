using UnityEngine;

public sealed class ThemePlay : MonoBehaviour
{
    [SerializeField]
    private AudioClip _clip;
    
    private void Start()
    {
        MusicManager.Instance.Play(_clip);
    }
}
