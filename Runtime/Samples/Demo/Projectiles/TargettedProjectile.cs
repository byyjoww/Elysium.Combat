using Elysium.Utils;
using Elysium.Utils.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public class TargettedProjectile : MonoBehaviour, IProjectile
    {
        [Separator("Settings", true)]
        [SerializeField] private float speed = 10f;
        [SerializeField] private float stoppingDistance = 0.1f;
        [SerializeField] private float overshootThreshold = 0f;
        [SerializeField] private float destroyTime = 0f;

        [Separator("Visual", true)]
        [SerializeField] private bool createExplosion = false;
        [SerializeField, ConditionalField("createExplosion")] private bool parentExplosionToTarget = false;
        [SerializeField, ConditionalField("createExplosion")] private GameObject explosion = default;

        public bool FriendlyFire { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Speed { get => speed; set => speed = value; }

        private Transform Target { get; set; }
        private Vector3 Origin { get; set; }
        private Vector3 TargetLastKnownPosition { get; set; }
        private UnityAction<IDamageable> OnHit { get; set; }
        private bool TargetIsStillValid => Target != null && Target && Target.gameObject.activeSelf;
        private Vector3 TargetPosition
        {
            get
            {
                if (TargetIsStillValid) { TargetLastKnownPosition = Target.position; }
                return TargetLastKnownPosition;
            }
        }

        public void Setup(Transform _target, UnityAction<IDamageable> _OnHit)
        {
            this.Origin = transform.position;
            this.OnHit = _OnHit;
            this.Target = _target;
        }

        public void Setup(Vector3 _direction, DamageTeam[] _dealsDamageTo, UnityAction<IDamageable> _OnHit)
        {
            throw new System.NotImplementedException();
        }

        private void Update()
        {
            Move();
            CheckDestination();
        }

        private void Move()
        {
            transform.LookAt(TargetPosition);
            transform.Translate(transform.InverseTransformDirection(transform.forward) * Time.deltaTime * speed);
        }

        private void CheckDestination()
        {
            // CHECK IF PROJECTILE OVERSHOT TARGET
            Vector3 line = TargetPosition - Origin;
            Vector3 worldRelativePosition = TargetPosition - transform.position;
            if (Vector3.Dot(line, worldRelativePosition) < overshootThreshold * line.sqrMagnitude || Vector3.Distance(transform.position, TargetPosition) < stoppingDistance)
            {
                Hit();
            }
        }

        private void Hit()
        {
            IDamageable damageable = Target.GetComponent<IDamageable>();
            OnHit?.Invoke(damageable);

            if (createExplosion) 
            { 
                if (parentExplosionToTarget) { Instantiate(explosion, Target); }
                else { Instantiate(explosion, Target.position, explosion.transform.rotation); }
            }

            Destroy(gameObject, destroyTime);
        }
    }
}
