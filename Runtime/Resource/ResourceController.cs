using Elysium.UI.ProgressBar;
using Elysium.Utils;
using Elysium.Utils.Attributes;
using Elysium.Utils.Timers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public class ResourceController : MonoBehaviour, IResourceController, IFillable
    {
        [SerializeField] private bool manual = false;
        [SerializeField, ConditionalField("manual")] private int value = default;
        [SerializeField] protected bool fillOnStart = false;
        [SerializeField, ReadOnly] private int currentResource = default;
        [SerializeField, ReadOnly] protected IResource resource = new NullResource();

        public float Max => resource.Max;
        public float Current => resource.Current;
        
        // EVENTS        
        public event UnityAction<int> OnResourceLost;
        public event UnityAction<int> OnResourceGained;
        public event UnityAction<int, int> OnChanged;
        public event UnityAction OnFillValueChanged;
        public event UnityAction OnEmpty;

        private void Start()
        {
            if (manual && resource is NullResource)
            {
                IResource res = new Resource(value, 0);
                Setup(res);
            }
        }

        protected virtual void Setup(IResource _resource)
        {
            if (resource == null) { throw new System.Exception("trying to setup null resource"); }

            this.resource = _resource;
            this.resource.OnChanged += TriggerOnChanged;
            this.resource.OnEmpty += TriggerOnEmpty;
            this.resource.OnFillValueChanged += TriggerOnFillValueChanged;
            this.resource.OnResourceGained += TriggerOnResourceGained;
            this.resource.OnResourceLost += TriggerOnResourceLost;

            if (fillOnStart) { Fill(); }
        }

        public virtual bool TryGain(int _amount)
        {
            return Gain(_amount);
        }

        public virtual bool TryLose(int _amount)
        {
            if (resource.Current < _amount) { return false; }
            return Lose(_amount);
        }

        public virtual bool ForceLose(int _amount)
        {
            return Lose(_amount);
        }

        public virtual bool Set(int _amount)
        {
            resource.Current = _amount;
            return true;
        }

        public virtual bool Fill()
        {
            resource.Fill();
            return true;
        }

        public virtual bool Empty()
        {
            resource.Empty();
            return true;
        }

        protected virtual bool Gain(int _amount)
        {
            resource.Gain(_amount);
            return true;
        }

        protected virtual bool Lose(int _amount)
        {
            resource.Lose(_amount);
            return true;
        }

        protected virtual void TriggerOnResourceGained(int _amount) => OnResourceGained?.Invoke(_amount);

        protected virtual void TriggerOnResourceLost(int _amount) => OnResourceLost?.Invoke(_amount);

        protected virtual void TriggerOnChanged(int _prev, int _current) => OnChanged?.Invoke(_prev, _current);

        protected virtual void TriggerOnEmpty() => OnEmpty?.Invoke();

        protected virtual void TriggerOnFillValueChanged()
        {
            currentResource = (int)resource.Current;
            OnFillValueChanged?.Invoke();
        }
    }
}
