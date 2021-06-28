using Elysium.Utils.Attributes;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace Elysium.Combat
{
    public class HealthController : ResourceController, IDamageable
    {
        [SerializeField, ConditionalField("manual")] private DamageTeam team = default;
        [SerializeField, ConditionalField("manual")] private Element element = default;
        public DamageTeam Team => team;
        public IElement Element { get; private set; } = new NullElement();
        public MonoBehaviour Controller => this;
        public bool IsDead { get; private set; }        

        public event UnityAction<int, ISource> OnTakeDamage;
        public event UnityAction<int, ISource> OnHeal;
        public event UnityAction OnDeath;
        public event UnityAction OnRespawn;

        protected override void Start()
        {
            base.Start();
            if (manual)
            {
                IElementFactory efactory = FindObjectsOfType<MonoBehaviour>().OfType<IElementFactory>().FirstOrDefault();
                if (efactory != null)
                {
                    Element = efactory.GetElementByKeyOrDefault(element);
                }
            }
        }

        public bool TakeDamage(int amount, ISource _source)
        {
            if (IsDead) 
            { 
                Debug.Log($"{gameObject.name} is dead"); 
                return false; 
            }

            Lose(amount, _source);
            OnTakeDamage?.Invoke(amount, _source);
            if (resource.Current <= 0) { Die(); }
            return true;
        }

        public bool Heal(int amount, ISource _source)
        {
            if (IsDead) { Debug.Log($"{gameObject.name} is dead"); return false; }

            Gain(amount, _source);
            OnHeal?.Invoke(amount, _source);
            if (resource.Current <= 0) { Die(); }
            return true;
        }

        public bool Ressurect(float _percentage, ISource _source)
        {
            return Ressurect(resource.Max * _percentage, _source);
        }

        public bool Ressurect(int _amount, ISource _source)
        {
            if (!IsDead)
            { 
                Debug.Log($"{gameObject.name} is not dead"); 
                return false; 
            }

            IsDead = false;
            OnRespawn?.Invoke();
            Gain(_amount, _source);
            return true;
        }

        public bool Kill(ISource _source)
        {
            Empty(_source);
            return Die();
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
