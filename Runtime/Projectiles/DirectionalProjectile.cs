using Elysium.Combat;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Combat
{
    public class DirectionalProjectile : GenericProjectile
    {
        protected override Vector3 targetPos
        {
            get
            {
                if (direction.HasValue)
                {
                    lastKnownPosition = direction.Value;
                    return lastKnownPosition;
                }
                else
                {
                    return lastKnownPosition;
                }
            }
        }

        public override void Move()
        {
            Vector3 direction = targetPos - origin;
            Debug.DrawRay(transform.position, direction, Color.red);

            Vector3 desiredTarget = direction + transform.position;
            desiredTarget.y = transform.position.y;
            transform.LookAt(desiredTarget);

            transform.Translate(transform.InverseTransformDirection(transform.forward) * Time.deltaTime * speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageable = other.gameObject.GetComponentInChildren<IDamageable>();
            if (damageable == null) { return; }
            if (!dealsDamageTo.Contains(damageable.Team)) { return; }

            // VALID TARGET
            OnHitTarget(damageable);
        }
    }
}