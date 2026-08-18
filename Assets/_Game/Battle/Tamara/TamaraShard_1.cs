using UnityEngine;

public sealed class TamaraShard_1 : MonoBehaviour
{
    [SerializeField]
    private float _force = 3;

    private bool _isMinus;
    private float _duration;
    private float _z;
    private float _y;
    private float _speedY = 4;
    private float _amplitude = 2;

    // бутылка
    
    private void Start()
    {
        transform.localScale = Vector3.one * 0.5f;
    }

    private void Update()
    {
        // if (!_isMinus)
        // {
        //     _z += 1300 * Time.deltaTime;
        // }
        // else
        // {
        //     _z -= 1300 * Time.deltaTime;
        // }
        
        transform.rotation = Quaternion.Euler(0f, 0f, 90);

        if ((BattleManager.Instance.Enemies[0] as Tamara).Mercy > 2 && Stats.Instance.HP <= 5)
        {
            transform.position += new Vector3(0, -Time.deltaTime * 3, 0f);
            
            if (Vector2.Distance(transform.position, Heart.Instance.transform.position) < 3)
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
                
                if (Heart.Instance.transform.position.x == transform.position.x)
                    transform.position += new Vector3(Random.Range(0, 2) == 0 ? 3 : -3 * Time.deltaTime, 0, 0);
                
                Debug.Log("Двигаем!!!");
            }
        }
        else
        {
            _duration += Time.deltaTime * 3;
            transform.position += new Vector3(0, -_speedY + Mathf.Cos(_y) * _amplitude, 0f) * Time.deltaTime;
            transform.position = new Vector3(Mathf.Sin(_duration) * _force, transform.position.y, 0f);

            _y += Time.deltaTime * 10;

            if (_speedY > 0)
                _speedY -= Time.deltaTime / 5;
            else
                _speedY = 0;

            if (transform.position.y <= -3.22f)
            {
                _y = 0;
                _speedY = 0;
                _amplitude = 0;
                _force = 3;
            }
        }
    }

    public void Init(bool minus, float speedY)
    {
        _isMinus = minus;
        _speedY = speedY;
        
        if (minus)
            _force = -_force;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(3);
        Destroy(gameObject);
    }
}
