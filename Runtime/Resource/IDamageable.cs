using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public interface IDamageable
    {
        bool IsDead { get; }
        DamageTeam Team { get; }
        MonoBehaviour Controller { get; }

        event Action<IDamageDealer, int, string> OnTakeDamage;
        event Action<IDamageDealer, int, string> OnHeal;
        event Action OnDeath;
        event Action OnRespawn;
        bool TakeDamage(IDamageDealer damageComponent, int damage, string source = "");
        bool Heal(IDamageDealer damageComponent, int damage, string source = "");

        bool Ressurect(int _amount);
        bool Ressurect(float _percentage);
        bool Kill();
    }
}