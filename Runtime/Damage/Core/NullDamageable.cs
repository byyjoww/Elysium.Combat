using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public class NullDamageable : IDamageable
    {
        public bool IsDead => false;

        public DamageTeam Team => default;

        public event UnityAction<int, ISource> OnTakeDamage;
        public event UnityAction<int, ISource> OnHeal;
        public event UnityAction OnDeath;
        public event UnityAction OnRespawn;

        public bool TakeDamage(int amount, ISource _source)
        {
            return true;
        }

        public bool Heal(int amount, ISource _source)
        {
            return true;
        }

        public bool Kill(ISource _source)
        {
            return false;
        }

        public bool Ressurect(int _amount, ISource _source)
        {
            return false;
        }

        public bool Ressurect(float _percentage, ISource _source)
        {
            return false;
        }        
    }
}