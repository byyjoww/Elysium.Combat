using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public interface IModelController
    {
        Transform Firepoint { get; }

        event UnityAction<string> OnAnimationHit;
        event UnityAction<string> OnAnimationEnd;

        void PlayAnimation(string _trigger);
        void SetAttackSpeed(float _aspd);

        Material SetMaterial(Material material);
    }
}
