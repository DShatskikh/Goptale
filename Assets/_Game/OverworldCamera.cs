using UnityEngine;

public sealed class OverworldCamera : MonoBehaviour
{
    public static OverworldCamera Instance;
    public bool IsDownLimit;
    public float DownLimit;
    public bool IsLeftLimit;
    public float LeftLimit;
    public bool IsRightLimit;
    public float RightLimit;
    public bool IsUpLimit;
    public float UpLimit;

    private void Awake()
    {
        Instance = GetComponent<OverworldCamera>();
    }

    private void Update()
    {
        transform.position = new Vector3(Fedya.Instance.transform.position.x, Fedya.Instance.transform.position.y + 1f, transform.position.z);

        if (IsUpLimit)
        {
            if (transform.position.y > UpLimit)
                transform.position = new Vector3(transform.position.x, UpLimit, transform.position.z);
        }
        
        if (IsDownLimit)
        {
            if (transform.position.y < DownLimit)
                transform.position = new Vector3(transform.position.x, DownLimit, transform.position.z);
        }
        
        if (IsLeftLimit)
        {
            if (transform.position.x < LeftLimit)
                transform.position = new Vector3(LeftLimit, transform.position.y, transform.position.z);
        }
        
        if (IsRightLimit)
        {
            if (transform.position.x > RightLimit)
                transform.position = new Vector3(RightLimit, transform.position.y, transform.position.z);
        }
    }
}
