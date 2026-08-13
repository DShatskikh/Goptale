using UnityEngine;

public sealed class DJNikolay_Shard_1 : MonoBehaviour
{
    private void Update()
    {
        transform.position += (-transform.position + Heart.Instance.transform.position).normalized * Time.deltaTime * 1.5f;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(3);
        Destroy(gameObject);
    }
}
