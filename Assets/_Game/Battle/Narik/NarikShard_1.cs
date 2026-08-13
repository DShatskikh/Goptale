using UnityEngine;

public sealed class NarikShard_1 : MonoBehaviour
{
    private const float SPEED = 7f;

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * SPEED;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(3);
        Destroy(gameObject);
    }
}
