using UnityEngine;

public sealed class Frame : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _up, _down, _left, _right;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _right.transform.localPosition = new Vector3(_spriteRenderer.size.x / 2, 0);
        _right.transform.localScale = new Vector3(0.1f, _spriteRenderer.size.y);
        
        _left.transform.localPosition = new Vector3(-_spriteRenderer.size.x / 2, 0);
        _left.transform.localScale = new Vector3(0.1f, _spriteRenderer.size.y);
        
        _up.transform.localPosition = new Vector3(0, _spriteRenderer.size.y / 2);
        _up.transform.localScale = new Vector3(_spriteRenderer.size.x, 0.1f); // 1.75f
        
        _down.transform.localPosition = new Vector3(0, -_spriteRenderer.size.y / 2);
        _down.transform.localScale = new Vector3(_spriteRenderer.size.x, 0.1f);
    }
}
