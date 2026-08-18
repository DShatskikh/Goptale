using UnityEngine;

public sealed class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public float GetTime => GetComponent<AudioSource>().time;
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip)
    {
        if (GetComponent<AudioSource>().clip ==  clip)
            return;

        if (clip == null)
        {
            Stop();
            return;
        }
        
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }

    public void Stop()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = null;
    }
    
    public void SetTime(float time)
    {
        GetComponent<AudioSource>().time = time;
    }
}
