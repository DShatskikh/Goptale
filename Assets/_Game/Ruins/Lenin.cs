using UnityEngine;

public class Lenin : MonoBehaviour
{
    [SerializeField]
    private float _targetX = 4.51f;

    public bool IsActivate;
    
    private void Update()
    {
        if (IsActivate)
        {
            transform.position = new Vector2(_targetX, transform.position.y);
            enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<Fedya>())
            return;

        if (!enabled)
            return;
        
        if (transform.position.x >= _targetX)
        {
            IsActivate = true;
            return;
        }
        
        transform.position += Vector3.right * 6 * 0.01f;
    }
}
