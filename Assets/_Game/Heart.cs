using UnityEngine;

public sealed class Determination : MonoBehaviour
{
    public static Determination Instance;

    private void Awake()
    {
        Instance = this;
    }
}
