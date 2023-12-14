using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(EnemyVision))]
[RequireComponent(typeof(EnemyMove))]
[RequireComponent(typeof(EnemyAttack))]

public class Samurai : MonoBehaviour, IHealthChanged
{
    [SerializeField] private Transform _castPoint;
    [SerializeField] private bool _isAttack;
    [SerializeField] private SpriteRenderer _samuraiDeath;

    private SpriteRenderer _spriteRenderer;
    private EnemyVision _enemyVision;
    private EnemyMove _enemyMove;
    private EnemyAttack _enemyAttack;
    private Player _player;
    private int _hitPoint;
    private int _hitPointMax;  
    public event UnityAction<int, int> HealthChanged;

    public Transform CastPoint => _castPoint;

    private void Awake()
    {
        _hitPointMax = 5;
        _hitPoint = _hitPointMax;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyVision = GetComponent<EnemyVision>();
        _enemyMove = GetComponent<EnemyMove>();
        _enemyAttack = GetComponent<EnemyAttack>();
    }

    private void Start()
    {
        HealthChanged.Invoke(_hitPoint, _hitPointMax);
    }

    void FixedUpdate()
    {
        if (_isAttack == false)
        {
            if (_player == null)
            {
                _enemyMove.Move();
            }
            else
            {
                LookPlayer();
                if (Vector2.Distance(transform.position, _player.transform.position) > _enemyAttack.DistanceAttack)
                {
                    _enemyMove.Move();
                }
                else
                {
                    _enemyAttack.Attack();
                }
            }
        }
    }

    public void ChangeDirection()
    {
        _castPoint.localPosition = -_castPoint.localPosition;

        if (_spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }

        _enemyVision.ChangeDirection();
        _enemyMove.ChangeDirection();
        _enemyAttack.ChangeDirection();
    }

    public void SetPlayer(Player player) 
    {
        _player = player;
    }

    public void TakeDamage(int damage)
    {
        _hitPoint -= damage;
       
        if(_hitPoint > 0)
        {
            HealthChanged.Invoke(_hitPoint, _hitPointMax);
        }
        else
        {
            SpriteRenderer samuraiDeath = Instantiate(_samuraiDeath, transform.position, Quaternion.identity);
            samuraiDeath.flipX = _spriteRenderer.flipX;
        
            Destroy(gameObject);
        }
    }

    public void Heal()
    {
        if (_hitPoint < _hitPointMax)
        {
            _hitPoint += 1;
            HealthChanged.Invoke(_hitPoint, _hitPointMax);
        }
    }

    private void LookPlayer()
    {
        if (_player.transform.position.x < transform.position.x)
        {
            if (_spriteRenderer.flipX != false)
            {
                ChangeDirection();
            }
        }
        else
        {
            if (_spriteRenderer.flipX == false)
            {
                ChangeDirection();
            }
        }
    }
}
