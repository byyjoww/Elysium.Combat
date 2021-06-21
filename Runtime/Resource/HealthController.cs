using Elysium.Utils.Attributes;
using System;
using UnityEngine;

namespace Elysium.Combat
{
    public class HealthController : ResourceController, IDamageable
    {        
        [SerializeField] private DamageTeam team;
        public DamageTeam Team => team;
        public MonoBehaviour Controller => this;
        public bool IsDead { get; private set; }

        public event Action<IDamageDealer, int, string> OnTakeDamage;
        public event Action<IDamageDealer, int, string> OnHeal;
        public event Action OnDeath;
        public event Action OnRespawn;

        public bool TakeDamage(IDamageDealer damageDealer, int amount, string source = "")
        {
            if (IsDead) 
            { 
                Debug.Log($"{gameObject.name} is dead"); 
                return false; 
            }

            ForceLose(amount);
            OnTakeDamage?.Invoke(damageDealer, amount, source);
            if (resource.Current <= 0) { Die(); }
            return true;
        }

        public bool Heal(IDamageDealer damageDealer, int amount, string source = "")
        {
            if (IsDead) { Debug.Log($"{gameObject.name} is dead"); return false; }

            Gain(amount);

            OnHeal?.Invoke(damageDealer, amount, source);
            if (resource.Current <= 0) { Die(); }
            return true;
        }

        public bool Ressurect(float _percentage)
        {
            return Ressurect(resource.Max * _percentage);
        }

        public bool Ressurect(int _amount)
        {
            if (!IsDead)
            { 
                Debug.Log($"{gameObject.name} is not dead"); 
                return false; 
            }

            IsDead = false;
            OnRespawn?.Invoke();
            return Gain(_amount);
        }

        public bool Kill()
        {
            if (Empty()) { return Die(); }
            return false;
        }

        private bool Die()
        {
            if (IsDead) 
            { 
                Debug.Log($"{gameObject.name} is already dead");                 
                return false; 
            }

            IsDead = true;
            OnDeath?.Invoke();
            return true;
        }
    }
}
