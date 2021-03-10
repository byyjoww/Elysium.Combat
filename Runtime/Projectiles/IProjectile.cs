using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    public interface IProjectile
    {
        void Setup(IDamageable _target, Vector3 _fallbackLocation, List<DamageTeam> _dealsDamageTo, Action<IDamageable> _onHit);
        void Setup(Vector3 _target, List<DamageTeam> _dealsDamageTo, Action<IDamageable> _onHit);

        void Move();
        void OnHitTarget(IDamageable _target);
    }
}