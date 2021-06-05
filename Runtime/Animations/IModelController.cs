using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public interface IModelController
    {
        event UnityAction<string> OnAnimationHit;
        event UnityAction<string> OnAnimationEnd;

        void PlayAnimation(string _trigger);
        void EndAnimation();

        void SetAttackSpeed(float _aspd);

        Material SetMaterial(Material material);
    }
}
