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

        public float Current { get => resource.Current; }
        public float Min { get => resource.Max; }
        public float Max { get => resource.Max; }

        public event UnityAction<int, ISource> OnResourceGained;
        public event UnityAction<int, ISource> OnResourceLost;
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

        public bool MustLose(int _amount, ISource _source) => resource.MustLose(_amount, _source);

        public bool MustGain(int _amount, ISource _source) => resource.MustGain(_amount, _source);

        public virtual void Gain(int _amount, ISource _source) => resource.Gain(_amount, _source);

        public virtual void Lose(int _amount, ISource _source) => resource.Lose(_amount, _source);

        public virtual void Fill(ISource _source) => resource.Fill(_source);

        public virtual void Empty(ISource _source) => resource.Empty(_source);

        public void SetCurrent(int _amount) => resource.SetCurrent(_amount);

        public void SetMin(int _amount) => resource.SetMin(_amount);

        public void SetMax(int _amount) => resource.SetMax(_amount);

        protected virtual void TriggerOnResourceGained(int _amount, ISource _source) => OnResourceGained?.Invoke(_amount, _source);

        protected virtual void TriggerOnResourceLost(int _amount, ISource _source) => OnResourceLost?.Invoke(_amount, _source);

        protected virtual void TriggerOnChanged(int _prev, int _current) => OnChanged?.Invoke(_prev, _current);

        protected virtual void TriggerOnEmpty() => OnEmpty?.Invoke();

        protected virtual void TriggerOnFillValueChanged() => OnFillValueChanged?.Invoke();        
    }
}