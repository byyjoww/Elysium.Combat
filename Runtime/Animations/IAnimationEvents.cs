using System;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public interface IAnimationEvents
    {
        event UnityAction OnAttackHit;
        event UnityAction OnAttackEnd;

        void SetAttackSpeed(float _speed);
        void StartAnimation(Animation _animation);
    }

    public enum Animation
    {
        Idle = 0,
        Walk = 1,
        Attack = 2,
        Block = 3,
        Jump = 4,
    }
}