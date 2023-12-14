using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHealthChanged
{
    public event UnityAction<int, int> HealthChanged;

    public void TakeDamage(int damage);

    public void Heal();
}
