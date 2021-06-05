using Elysium.Utils;
using Elysium.Utils.Attributes;
using Elysium.Utils.Timers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public class ResourceController : MonoBehaviour, IResource
    {
        public RefValue<int> MaxResource { get; set; } = new RefValue<int>(() => 100);
        public RefValue<int> PassiveRecoveryAmount { get; set; } = new RefValue<int>(() => 0);
        public RefValue<float> PassiveRecoveryInterval { get; set; } = new RefValue<float>(() => 0);

        [SerializeField, ReadOnly] protected int currentResource;

        public float Max => MaxResource.Value;
        public float Current => currentResource;

        public bool PassiveRecoveryEnabled { get; set; } = false;
        protected TimerInstance passiveRecoveryTimer;

        // EVENTS
        public event UnityAction OnFillValueChanged;
        public event UnityAction<int> OnResourceLost;
        public event UnityAction<int> OnResourceGained;
        public event UnityAction<int, int> OnChanged;

        public virtual bool TryGain(int _amount)
        {
            return Gain(_amount);
        }

        public virtual bool TryGainPassive(int _amount)
        {
            if (!PassiveRecoveryEnabled) { return false; }
            return Gain(_amount);
        }

        public virtual bool TryLose(int _amount)
        {
            if (currentResource < _amount) { return false; }
            return Lose(_amount);
        }

        public virtual bool ForceLose(int _amount)
        {
            return Lose(_amount);
        }

        public virtual bool Set(int _amount)
        {
            int prev = currentResource;
            _amount = Mathf.Clamp(_amount, 0, MaxResource.Value);
            currentResource = _amount;
            OnFillValueChanged?.Invoke();
            OnChanged?.Invoke(prev, currentResource);
            return true;
        }

        public virtual bool Fill()
        {
            int prev = currentResource;
            currentResource = MaxResource.Value;
            OnFillValueChanged?.Invoke();
            OnChanged?.Invoke(prev, currentResource);
            return true;
        }

        protected virtual void Start()
        {
            SetupPassiveRecovery();
        }

        protected virtual void SetupPassiveRecovery()
        {
            passiveRecoveryTimer = Timer.CreateTimer(PassiveRecoveryInterval.Value, () => !this, false);

            void Tick()
            {
                TryGainPassive(PassiveRecoveryAmount.Value);
                passiveRecoveryTimer.SetTime(PassiveRecoveryInterval.Value);
            }

            passiveRecoveryTimer.OnTimerEnd += Tick;
            PassiveRecoveryEnabled = true;
        }

        protected virtual bool Gain(int _amount)
        {
            int prev = currentResource;
            currentResource = Mathf.Clamp(currentResource + _amount, 0, MaxResource.Value);
            int added = currentResource - prev;
            // Debug.Log($"Gains {_amount} {gameObject.name} | Before: {prev} | After: {currentResource}.");

            if (added == 0) { return false; }

            OnResourceGained?.Invoke(added);
            OnChanged?.Invoke(prev, currentResource);
            OnFillValueChanged?.Invoke();

            return true;
        }

        protected virtual bool Lose(int _amount)
        {
            int prev = currentResource;
            currentResource = Mathf.Clamp(currentResource - _amount, 0, MaxResource.Value);
            int deducted = prev - currentResource;
            // Debug.Log($"Loses {_amount} {gameObject.name} | Before: {prev} | After: {currentResource}.");

            if (deducted == 0) { return false; }

            OnResourceLost?.Invoke(deducted);
            OnChanged?.Invoke(prev, currentResource);
            OnFillValueChanged?.Invoke();
            return true;
        }

        public virtual void TriggerOnFillValueChanged()
        {
            // FORCE RECALCULATE THE MAX VALUE
            MaxResource.Recalculate();
            OnFillValueChanged?.Invoke();
        }
    }
}
