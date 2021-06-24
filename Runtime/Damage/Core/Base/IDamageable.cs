using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public interface IDamageable
    {
        bool IsDead { get; }
        DamageTeam Team { get; }

        event UnityAction<int, ISource> OnTakeDamage;
        event UnityAction<int, ISource> OnHeal;
        event UnityAction OnDeath;
        event UnityAction OnRespawn;

        bool TakeDamage(int amount, ISource _source);
        bool Heal(int amount, ISource _source);

        bool Ressurect(int _amount, ISource _source);
        bool Ressurect(float _percentage, ISource _source);
        bool Kill(ISource _source);
    }
}