using System;
using System.Collections;
using UnityEngine;

public sealed class Fedya : MonoBehaviour
{
    public static Fedya Instance;
    public static bool IsLoad;

    [SerializeField]
    private float _speed = 3;

    [SerializeField]
    private SpriteRenderer _view;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private StatsWindow _statsWindow;

    [SerializeField]
    private AudioSource _startWaterSFX, _stepWaterSFX;

    private Rigidbody2D _rigidbody;
    private Vector2 _previousPosition;
    private Vector2 _previousInput;
    private bool _isWater;
    private float _waterStep = 0.5f;
    public GameObject Danger;
    public bool IsMove => _animator.GetFloat("Speed") > 0;
    public SpriteRenderer View  => _view;

    private void Awake()
    {
        Instance = this;
        View.enabled = false;
        
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _previousPosition = transform.position;
    }

    private void OnDisable()
    {
        _animator.SetFloat("Speed", 0);
        _rigidbody.linearVelocity = Vector2.zero;
        _previousInput = Vector2.zero;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => IsLoad);
        View.enabled = true;
    }

    private void Update()
    {
        if (InputManager.Instance.IsOpenInventoryDown)
        {
            enabled = false;
            _statsWindow.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
            _statsWindow.gameObject.SetActive(true);
            return;
        }
        
        var input = new Vector2(InputManager.Instance.Horizontal, InputManager.Instance.Vertical);
        
        if (input != Vector2.zero && (InputManager.Instance.VerticalUp || InputManager.Instance.HorizontalUp || _previousInput == Vector2.zero))
        {
            _animator.SetFloat("Horizontal", input.x);
            _animator.SetFloat("Vertical", input.y);
        }
        
        _previousInput = input;

        if (InputManager.Instance.IsSubmitDown)
        {
            GetNearestUsable()?.Use();
        }
        
        var isWater = Physics2D.OverlapCircleAll(transform.position, 0.1f, LayerMask.GetMask("Water")).Length > 0;
        _animator.SetBool("IsWater", isWater);

        if (isWater && IsMove)
        {
            _waterStep -= Time.deltaTime;

            if (_waterStep < 0)
            {
                _waterStep = 0.5f;
                _stepWaterSFX.Play();
            }
        }
        else
        {
            _waterStep = 0.5f;
        }
        
        if (isWater && !_isWater)
        {
            _startWaterSFX.Play();
        }
        
        _isWater = isWater;
    }

    private void FixedUpdate()
    {
        var input = new Vector2(InputManager.Instance.Horizontal, InputManager.Instance.Vertical);
        _rigidbody.linearVelocity = input * _speed;
        var step = _previousPosition - _rigidbody.position;
        var stepDirection = -step.normalized;

        if (step != Vector2.zero)
        {
            _animator.SetFloat("Speed", 1);
            Stats.Instance.Position = transform.position;

#if PLATFORM_ANDROID
            if (Mathf.Abs(input.x) < Mathf.Abs(input.y))
            {
                _animator.SetFloat("Horizontal", 0);
                _animator.SetFloat("Vertical", input.y);
            }
        
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                _animator.SetFloat("Horizontal", input.x);
                _animator.SetFloat("Vertical", 0);
            } 
#endif
            
        }
        else
        {
            _animator.SetFloat("Speed", 0);
        }
        
        _previousPosition = transform.position;
    }

    public void SetFlex(bool value)
    {
        _animator.SetFloat("Horizontal", value ? 1 : 0);
    }
    
    public void SetDirection(Vector2 value)
    {
        _animator.SetFloat("Horizontal", value.x);
        _animator.SetFloat("Vertical", value.y);
    }

    private Usable GetNearestUsable()
    {
        var collisions = Physics2D.OverlapCircleAll(transform.position, 0.5f);

        Usable closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (var collision in collisions)
        {
            if (collision.GetComponent<Usable>() != null)
            {
                float distance = Vector2.Distance(transform.position, collision.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = collision.gameObject.GetComponent<Usable>();
                }
            }
        }

        return closestObject;
    }
}
