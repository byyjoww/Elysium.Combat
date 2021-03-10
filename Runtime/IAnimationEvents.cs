using System;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public interface IAnimationEvents
    {
        event UnityAction OnAttackHit;
        event UnityAction OnAttackEnd;
    }
}