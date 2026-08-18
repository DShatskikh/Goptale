using UnityEngine;

public sealed class TamaraShard_4 : MonoBehaviour
{
    private float _startX;
    private float _duration;

    private void Start()
    {
        _startX = transform.position.x;
        transform.localScale = Vector3.one * 0.5f;
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    private void Update()
    {
        _duration += Time.deltaTime * 3;
        transform.position += new Vector3(0, -3, 0f) * Time.deltaTime;
        transform.position = new Vector3(_startX + Mathf.Sin(_duration) * 1, transform.position.y, 0f);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<Heart>())
            return;
        
        Heart.Instance.Damage(3);
        Destroy(gameObject);
    }
}
