using Elysium.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public class TargettedProjectile : GenericProjectile
    {
        [SerializeField] protected Vector3 offset = Vector3.zero;

        protected override Vector3 targetPos
        {
            get
            {
                if (target != null && !target.DamageableObject.IsDestroyed())
                {
                    if (target.IsDead)
                    {
                        target = null;
                        return lastKnownPosition;
                    }

                    lastKnownPosition = target.DamageableObject.transform.position;
                    return lastKnownPosition;
                }
                else
                {
                    return lastKnownPosition;
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            CheckDestination();
        }

        public override void Move()
        {
            transform.LookAt(targetPos);
            transform.Translate(transform.InverseTransformDirection(transform.forward) * Time.deltaTime * speed);
        }

        protected virtual void CheckDestination()
        {
            // CHECK IF PROJECTILE OVERSHOT TARGET
            Vector3 line = targetPos - origin;
            Vector3 worldRelativePosition = targetPos + offset - transform.position;

            if (Vector3.Dot(line, worldRelativePosition) < 0f * line.sqrMagnitude || Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                OnHitTarget(target);
            }
        }
    }
}
