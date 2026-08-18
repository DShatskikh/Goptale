using UnityEngine;

public sealed class TamaraShard_3 : MonoBehaviour
{
    private bool _isUp;
    private float _speed = 3;

    private void Start()
    {
        transform.localScale = Vector3.one * 0.5f;
    }
    
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 90);
        
        if ((BattleManager.Instance.Enemies[0] as Tamara).Mercy > 2 && Stats.Instance.HP <= 5)
        {
            if (Vector2.Distance(transform.position, Heart.Instance.transform.position) < 5)
            {
                var speed = 2;

                if (Vector2.Distance(transform.position, Heart.Instance.transform.position) < 1)
                {
                    speed = 7;
                }
                
                if (Heart.Instance.transform.position.x > transform.position.x)
                    transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                
                if (Heart.Instance.transform.position.x < transform.position.x)
                    transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }

            transform.position -= new Vector3(0, 1, 0f) * Time.deltaTime * _speed;
        }
        else
        {
            if (_isUp)
                transform.position += new Vector3(0, 1, 0f) * Time.deltaTime * _speed;
            else
                transform.position -= new Vector3(0, 1, 0f) * Time.deltaTime * _speed;

            if (transform.position.y > -0.773)
            {
                _isUp = false;
            }
            else if (transform.position.y < -3.23)
            {
                _isUp = true;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(3);
        Destroy(gameObject);
    }
}
