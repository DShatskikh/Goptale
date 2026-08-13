using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Heart : MonoBehaviour
{
    public static Heart Instance;

    [SerializeField]
    private AudioSource _hurtSFX, _healSFX;

    [SerializeField]
    private float _speed = 3;
    
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private bool _isInvisible;
    private float _invisibleTimer;

    private void Awake()
    {
        Instance = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _animator.Play("Heart");
    }

    private void Update()
    {
        if (_isInvisible)
        {
            _invisibleTimer -= Time.deltaTime;

            if (_invisibleTimer <= 0)
            {
                _isInvisible = false;
                _animator.Play("Heart");
            }
        }
    }

    private void FixedUpdate()
    {
        if (!BattleManager.Instance.IsEnemyTurn)
            return;
        
        _rigidbody2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * _speed;
    }

    public void Damage(int damage)
    {
        if (Stats.Instance.HP <= damage)
        {
            GameOver.Position = transform.position;
            SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
        }
        else if (!_isInvisible)
        {
            _isInvisible = true;
            Stats.Instance.HP -= damage;
            _invisibleTimer = 2;
            _animator.Play("Heart Damage");
            _hurtSFX.Play();
        }
    }

    public void Heal(int health)
    {
        Stats.Instance.HP += health;
        _healSFX.Play();
        
        if (Stats.Instance.HP > Stats.Instance.MaxHP)
            Stats.Instance.HP = Stats.Instance.MaxHP;
    }
}
