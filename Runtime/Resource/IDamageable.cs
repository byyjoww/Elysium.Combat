using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public interface IDamageable
    {
        event Action OnDeathStatusChange;

        bool IsDead { get; }
        DamageTeam Team { get; }
        GameObject DamageableObject { get; }
        
        bool TakeDamage(IDamageDealer damageComponent, int damage);
        bool Heal(IDamageDealer damageComponent, int damage);
    }
}