using UnityEngine;

public class TomaraShard_5 : MonoBehaviour
{
    private const float SPEED = 3;
    private Vector2 _direction;
    private Vector2 _startDirection;

    private void Start()
    {
        _direction = (Heart.Instance.transform.position - transform.position).normalized;
        _startDirection = _direction;
    }

    private void Update()
    {
        if (transform.localPosition.x < -0.4626f)
        {
            _direction.x = _startDirection.x;
        }
        
        if (transform.localPosition.y > 0.377f)
        {
            _direction.y = _startDirection.y;
        }
        
        if (transform.localPosition.x > 0.4626f)
        {
            _direction.x = -_startDirection.x;
        }
        
        if (transform.localPosition.y < -0.377f)
        {
            _direction.y = -_startDirection.y;
        }
        
        transform.position += (Vector3)_direction * SPEED * Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(3);
        Destroy(gameObject);
    }
}
