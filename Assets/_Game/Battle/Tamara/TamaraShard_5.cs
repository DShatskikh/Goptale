using UnityEngine;

public class TamaraShard_5 : MonoBehaviour
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

            if (_direction.x < 0)
                _direction.x = Random.Range(0.1f, 1f);
        }
        
        if (transform.localPosition.y > 0.377f)
        {
            _direction.y = _startDirection.y;
            
            if (_direction.y > 0)
                _direction.y = -Random.Range(0.1f, 1f);
        }
        
        if (transform.localPosition.x > 0.4626f)
        {
            _direction.x = -_startDirection.x;
            
            if (_direction.x > 0)
                _direction.x = -Random.Range(0.1f, 1f);
        }
        
        if (transform.localPosition.y < -0.377f)
        {
            _direction.y = Random.Range(0.1f, 1f);
            
            if (_direction.y < 0)
                _direction.y = 1;
        }
        
        _direction.Normalize();
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
