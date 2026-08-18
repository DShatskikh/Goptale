using UnityEngine;

public sealed class LVLabel : MonoBehaviour
{
    private void Start()
    {
        if (Stats.Instance.Name.Length >= 6)
            transform.localPosition = new Vector3(-2.933f, transform.localPosition.y);
    }
}
