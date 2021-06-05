using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public abstract class GenericProjectile : MonoBehaviour, IProjectile
    {
        protected bool initialized = false;

        [SerializeField] protected float speed = 10;

        protected IDamageable target = default;
        protected Vector3? direction = default;

        protected Vector3 lastKnownPosition = default;
        protected Vector3 origin = default;

        protected DamageTeam[] dealsDamageTo = default;

        protected Action<IDamageable> onHit = default;

        protected virtual Vector3 targetPos => Vector3.forward;

        public virtual void Setup(IDamageable _target, DamageTeam[] _dealsDamageTo, Action<IDamageable> _onHit)
        {
            if (_target == null) { Debug.LogError("passed in null target to projectile"); }
            lastKnownPosition = _target.DamageableObject.transform.position;
            target = _target;
            Setup(_dealsDamageTo, _onHit);
        }

        public virtual void Setup(Vector3 _direction, DamageTeam[] _dealsDamageTo, Action<IDamageable> _onHit)
        {
            direction = _direction;
            Setup(_dealsDamageTo, _onHit);
        }

        protected virtual void Setup(DamageTeam[] _dealsDamageTo, Action<IDamageable> _onHit)
        {
            onHit = _onHit;
            dealsDamageTo = _dealsDamageTo;
            origin = transform.position;
            initialized = true;
        }

        protected virtual void Update()
        {
            if (!initialized) { return; }
            Move();
        }

        public abstract void Move();

        public virtual void OnHitTarget(IDamageable _target)
        {
            onHit?.Invoke(_target);
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(targetPos, 0.2f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, targetPos);
        }
    }
}