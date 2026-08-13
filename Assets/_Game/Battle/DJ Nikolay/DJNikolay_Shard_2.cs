using UnityEngine;

public sealed class DJNikolay_Shard_2 : MonoBehaviour
{
    private void Update()
    {
        transform.position += new Vector3(0, -1f) * Time.deltaTime * 2;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(5);
        Destroy(gameObject);
    }
}
