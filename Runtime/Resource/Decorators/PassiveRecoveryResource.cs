using Elysium.Utils;
using Elysium.Utils.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Combat
{
    [System.Serializable]
    public class PassiveRecoveryResource : ResourceDecorator
    {
        private IElement element = new NullElement();
        private IDamageDealer self = default;
        private RefValue<int> recoveryAmount = new RefValue<int>(() => 0);
        private TimerInstance timer = default;

        public PassiveRecoveryResource(IResource _resource, float _interval, int _amount, IDamageDealer _self, IElement _element = null) : base(_resource)
        {
            this.self = _self;
            this.element = _element != null ? _element : new NullElement();
            this.recoveryAmount = new RefValue<int>(() => _amount);
            CreateTimer(_interval);
        }        

        public PassiveRecoveryResource(IResource _resource, float _interval, float _percentage) : base(_resource)
        {
            this.recoveryAmount = new RefValue<int>(() => Mathf.CeilToInt(Max * _percentage));
            CreateTimer(_interval);
        }

        private void CreateTimer(float _interval)
        {
            timer = Timer.CreateTimer(_interval, () => false, true);
            timer.OnEnd += Recover;
        }

        private void Recover()
        {
            Gain(recoveryAmount.Value, ResourceModifierSourceFactory.Unit(self, element));
        }
    }

    public static class PassiveRecoveryResourceExtension
    {
        public static IResource WithPassiveRecovery(this IResource _resource, float _interval, int _amount)
        {
            return new PassiveRecoveryResource(_resource, _interval, _amount);
        }

        public static IResource WithPassiveRecovery(this IResource _resource, float _interval, float _percentage)
        {
            return new PassiveRecoveryResource(_resource, _interval, _percentage);
        }
    }
}