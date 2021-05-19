using Elysium.Utils.Attributes;
using System;
using UnityEngine;

namespace Elysium.Combat
{
    [Serializable]
    public class HealthController : ResourceController, IDamageable
    {
        [ReadOnly] public DamageTeam DamageTeam;
        [SerializeField, ReadOnly] private bool isDead;

        public DamageTeam Team => DamageTeam;
        public bool IsDead => isDead;

        private GameObject damageableObject;
        public GameObject DamageableObject
        {
            get
            {
                if (damageableObject == null) { return null; }
                return damageableObject;
            }
        }

        // EVENTS
        public event Action OnHealthEmpty;
        public event Action<IDamageDealer, int> OnTakeDamage;
        public event Action<IDamageDealer, int> OnHeal;
        public event Action OnDeathStatusChange;
        public event Action OnDeath;
        public event Action OnRespawn;

        private void Awake() => damageableObject = transform.parent.gameObject;

        public bool TakeDamage(IDamageDealer damageDealer, int amount)
        {
            if (IsDead) { Debug.Log($"{gameObject.name} is dead"); return false; }

            ForceLose(amount);

            OnTakeDamage?.Invoke(damageDealer, amount);
            CheckDeathStatus();
            return true;
        }

        public bool Heal(IDamageDealer damageDealer, int amount)
        {
            if (IsDead) { Debug.Log($"{gameObject.name} is dead"); return false; }

            Gain(amount);

            OnHeal?.Invoke(damageDealer, amount);
            CheckDeathStatus();
            return true;
        }

        public bool Ressurect()
        {
            if (!IsDead) { Debug.Log($"{gameObject.name} is not dead"); return false; }

            isDead = false;
            PassiveRecoveryEnabled = !isDead;
            Fill();

            OnDeathStatusChange?.Invoke();
            OnRespawn?.Invoke();
            return true;
        }

        private bool Die()
        {
            if (IsDead) { Debug.Log($"{gameObject.name} is already dead"); return false; }

            isDead = true;
            PassiveRecoveryEnabled = !isDead;
            OnDeath?.Invoke();
            OnDeathStatusChange?.Invoke();
            return true;
        }

        private void CheckDeathStatus()
        {
            if (currentResource <= 0)
            {
                currentResource = 0;
                OnHealthEmpty?.Invoke();
                Die();
            }
        }
    }
}