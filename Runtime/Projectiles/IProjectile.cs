using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public interface IProjectile
    {
        bool FriendlyFire { get; set; }
        float Speed { get; set; }

        void Setup(Vector3 _direction, DamageTeam[] _dealsDamageTo, UnityAction<IDamageable> _OnHit);
        void Setup(Transform _target, UnityAction<IDamageable> _OnHit);
    }
}