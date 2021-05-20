using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public interface IDamageable
    {
        event Action<IDamageDealer, int, string> OnTakeDamage;
        event Action<IDamageDealer, int, string> OnHeal;
        event Action OnHealthEmpty;
        event Action OnDeathStatusChange;
        event Action OnDeath;
        event Action OnRespawn;        

        bool IsDead { get; }
        DamageTeam Team { get; }
        GameObject DamageableObject { get; }
        
        bool TakeDamage(IDamageDealer damageComponent, int damage, string source = "");
        bool Heal(IDamageDealer damageComponent, int damage, string source = "");
    }
}