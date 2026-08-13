using UnityEngine;

public sealed class GopnikShard_1 : MonoBehaviour
{
    [SerializeField]
    private GameObject _parent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(3);
        Destroy(_parent);
    }
}
