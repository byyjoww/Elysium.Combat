using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    [System.Serializable]
    public class ResourceDecorator : IResource
    {
        [SerializeField] protected IResource resource = default;

        public float Current { get => resource.Current; set => resource.Current = value; }
        public float Max { get => resource.Max; set => resource.Max = value; }

        public event UnityAction<int> OnResourceGained;
        public event UnityAction<int> OnResourceLost;
        public event UnityAction<int, int> OnChanged;
        public event UnityAction OnEmpty;
        public event UnityAction OnFillValueChanged;        

        public ResourceDecorator(IResource _resource)
        {
            this.resource = _resource;
            if (this.resource != null) 
            {
                this.resource.OnChanged += TriggerOnChanged;
                this.resource.OnEmpty += TriggerOnEmpty;
                this.resource.OnFillValueChanged += TriggerOnFillValueChanged;
                this.resource.OnResourceGained += TriggerOnResourceGained;
                this.resource.OnResourceLost += TriggerOnResourceLost;
            }            
        }
        public virtual void Gain(int _amount) => resource.Gain(_amount);

        public virtual void Lose(int _amount) => resource.Lose(_amount);

        public virtual void Fill() => resource.Fill();

        public virtual void Empty() => resource.Empty();        

        protected virtual void TriggerOnResourceGained(int _amount) => OnResourceGained?.Invoke(_amount);

        protected virtual void TriggerOnResourceLost(int _amount) => OnResourceLost?.Invoke(_amount);

        protected virtual void TriggerOnChanged(int _prev, int _current) => OnChanged?.Invoke(_prev, _current);

        protected virtual void TriggerOnEmpty() => OnEmpty?.Invoke();

        protected virtual void TriggerOnFillValueChanged() => OnFillValueChanged?.Invoke();        
    }
}