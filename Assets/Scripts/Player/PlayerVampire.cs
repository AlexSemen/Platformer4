using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerVampire : MonoBehaviour
{
    [SerializeField] LayerMask _targetMask;
    [SerializeField] private float _radius;

    private int _damage;
    private Collider2D[] _enemys;
    private WaitForSeconds _waitForDelay;
    private float _delayTime;
    private int _repetitions;
    private Coroutine _fadeInJob;
    private Player _player;
    private bool _isVampirize;

    private void Awake()
    {
        _isVampirize = false;
        _damage = 1;
        _delayTime = 1;
        _repetitions = 6;
        _waitForDelay = new WaitForSeconds(_delayTime);
        _player = GetComponent<Player>();
    }

    public void VampireAttack()
    {
        if (_isVampirize == false)
        {
            _isVampirize = true;

            if (_fadeInJob != null)
            {
                StopCoroutine(_fadeInJob);
            }

            _fadeInJob = StartCoroutine(PullingHealth());
        }
    }

    private IEnumerator PullingHealth()
    {
        for (int i = 0; i < _repetitions; i++)
        {
            _enemys = Physics2D.OverlapCircleAll(transform.position, _radius, _targetMask);

            foreach(Collider2D enemy in _enemys)
            {
                if (enemy.TryGetComponent<Samurai>(out var samurai))
                {
                    samurai.TakeDamage(_damage);
                    _player.Heal();
                }
            }

            yield return _waitForDelay;
        }

        _isVampirize = false;
    }
}
