using UnityEngine;

public sealed class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;
    public float Timer;
    
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        Timer += Time.deltaTime;
    }
}
