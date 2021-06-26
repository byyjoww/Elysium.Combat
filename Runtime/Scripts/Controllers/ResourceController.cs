using Elysium.UI.ProgressBar;
using Elysium.Utils;
using Elysium.Utils.Attributes;
using Elysium.Utils.Timers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    public class ResourceController : MonoBehaviour, IResource, IFillable
    {
        [Separator("Resource", true)]
        [SerializeField, ReadOnly] private int currentResource = default;        
        [SerializeField, ConditionalField("manual")] private int min = default;
        [SerializeField, ConditionalField("manual")] private int max = default;

        [Separator("Options", true)]
        [SerializeField] private bool manual = false;        
        [SerializeField] protected bool fillOnStart = false;
        [SerializeField] private bool generatePopups = false;
        
        protected IResource resource = new NullResource();

        public float Current => resource.Current;
        public float Min => resource.Min;
        public float Max => resource.Max;
        
        // EVENTS        
        public event UnityAction<int, ISource> OnResourceLost;
        public event UnityAction<int, ISource> OnResourceGained;
        public event UnityAction<int, int> OnChanged;
        public event UnityAction OnFillValueChanged;
        public event UnityAction OnEmpty;

        protected virtual void Start()
        {
            if (manual && resource is NullResource)
            {
                IResource res = new Resource(max, min);
                if (generatePopups) { res = res.WithDamagePopup(transform); }
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

            if (fillOnStart) { Fill(SourceFactory.System); }
        }

        public virtual void Gain(int _amount, ISource _source)
        {
            resource.Gain(_amount, _source);
        }

        public virtual bool MustGain(int _amount, ISource _source)
        {
            return resource.MustGain(_amount, _source);
        }

        public virtual void Lose(int _amount, ISource _source)
        {
            resource.Lose(_amount, _source);
        }

        public virtual bool MustLose(int _amount, ISource _source)
        {
            return resource.MustLose(_amount, _source);
        }

        public virtual void Fill(ISource _source)
        {
            resource.Fill(_source);
        }

        public virtual void Empty(ISource _source)
        {
            resource.Empty(_source);
        }

        public virtual void SetCurrent(int _amount)
        {
            resource.SetCurrent(_amount);
        }

        public virtual void SetMin(int _amount)
        {
            resource.SetMin(_amount);
        }

        public virtual void SetMax(int _amount)
        {
            resource.SetMax(_amount);
        }

        protected virtual void TriggerOnResourceGained(int _amount, ISource _source) => OnResourceGained?.Invoke(_amount, _source);

        protected virtual void TriggerOnResourceLost(int _amount, ISource _source) => OnResourceLost?.Invoke(_amount, _source);

        protected virtual void TriggerOnChanged(int _prev, int _current) => OnChanged?.Invoke(_prev, _current);

        protected virtual void TriggerOnEmpty() => OnEmpty?.Invoke();

        protected virtual void TriggerOnFillValueChanged()
        {
            currentResource = (int)resource.Current;
            OnFillValueChanged?.Invoke();
        }
    }
}
