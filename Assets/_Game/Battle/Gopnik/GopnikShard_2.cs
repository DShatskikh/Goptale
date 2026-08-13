using System.Collections;
using UnityEngine;

public sealed class GopnikShard_2 : MonoBehaviour
{
    private const float SPEED = 5f;
    
    private Vector2 _direction;
    private bool _isDamage;
    public float Speed = SPEED;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        _direction = (-transform.position + Heart.Instance.transform.position).normalized;
    }

    private void Update()
    {
        transform.position += (Vector3)_direction * Time.deltaTime * Speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        if (_isDamage)
            return;

        _isDamage = true;
        Heart.Instance.Damage(3);
        Destroy(gameObject);
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
}
