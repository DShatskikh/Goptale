using UnityEngine;

public sealed class Reflection : MonoBehaviour
{
    [SerializeField]
    private float _value = 8.28f + 7.73f;
    
    private Vector2 _previousInput;
    private Animator _animator;
    private Vector3 _previousPosition;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = new Vector3(Fedya.Instance.transform.position.x, _value - Fedya.Instance.transform.position.y);
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if (input != Vector2.zero && (Input.GetButtonUp("Vertical") || Input.GetButtonUp("Horizontal") || _previousInput == Vector2.zero))
        {
            _animator.SetFloat("Horizontal", input.x);
            _animator.SetFloat("Vertical", -input.y);
        }
        
        if (_previousPosition - transform.position != Vector3.zero)
        {
            _animator.SetFloat("Speed", 1);
        }
        else
        {
            _animator.SetFloat("Speed", 0);
        }
    }

    private void FixedUpdate()
    {
        _previousPosition = transform.position;
    }
}
