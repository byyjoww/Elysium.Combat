using Elysium.Combat;
using Elysium.Utils.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    [RequireComponent(typeof(Collider))]
    public class DirectionalProjectile : MonoBehaviour, IProjectile
    {
        [Separator("Settings", true)]
        [SerializeField] private float speed = 10f;
        [SerializeField] private bool friendlyFire = false;
        [SerializeField] private LayerMask collidesWith = default;
        [SerializeField] private float stoppingDistance = 0.1f;
        [SerializeField] private float overshootThreshold = 0f;
        [SerializeField] private float destroyTime = 0f;

        [Separator("Visual", true)]
        [SerializeField] private bool createExplosion = true;
        [SerializeField, ConditionalField("createExplosion")] private bool parentExplosionToTarget = true;
        [SerializeField, ConditionalField("createExplosion")] private GameObject explosion = default;

        public float Speed { get => speed; set => speed = value; }
        public bool FriendlyFire { get => friendlyFire; set => friendlyFire = value; }

        private Vector3 Origin { get; set; }
        private UnityAction<IDamageable> OnHit { get; set; }
        private DamageTeam[] DealsDamageTo { get; set; } = new DamageTeam[] { };

        public void Setup(Vector3 _direction, DamageTeam[] _dealsDamageTo, UnityAction<IDamageable> _OnHit)
        {
            this.Origin = transform.position;
            this.OnHit = _OnHit;
            this.DealsDamageTo = _dealsDamageTo;

            transform.LookAt(Origin + _direction);
        }

        public void Setup(Transform _target, UnityAction<IDamageable> _OnHit)
        {
            this.Origin = transform.position;
            this.OnHit = _OnHit;
            DamageTeam dealsDamageTo = _target.GetComponentInChildren<IDamageable>().Team;
            this.DealsDamageTo = new DamageTeam[] { dealsDamageTo };

            Vector3 direction = (transform.position - _target.position).normalized;
            transform.LookAt(Origin + direction);
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            transform.Translate(transform.InverseTransformDirection(transform.forward) * Time.deltaTime * speed);
        }

        private void OnTriggerEnter(Collider _collider)
        {
            bool isInCollisionLayer = collidesWith.value == (collidesWith.value | (1 << _collider.gameObject.layer));
            if (isInCollisionLayer && IsValidTarget(_collider.transform, out IDamageable _damageable))
            {
                Hit(_collider.transform, _damageable);
            }
        }

        private bool IsValidTarget(Transform _transformHit, out IDamageable _damageable)
        {
            _damageable = _transformHit.GetComponentInChildren<IDamageable>();
            return _damageable == null || DealsDamageTo.Contains(_damageable.Team);
        }

        private void Hit(Transform _transformHit, IDamageable _damageableComponent)
        {
            OnHit?.Invoke(_damageableComponent);

            if (createExplosion)
            {
                if (parentExplosionToTarget) { Instantiate(explosion, _transformHit.transform); }
                else { Instantiate(explosion, _transformHit.position, explosion.transform.rotation); }
            }

            Destroy(gameObject, destroyTime);
        }
    }
}