using Elysium.UI.ProgressBar;
using Elysium.Utils.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Elysium.Combat
{
    [System.Serializable]
    public class Resource : IFillable, IResource
    {
        [SerializeField, ReadOnly] private int max = 1;
        [SerializeField, ReadOnly] private int current = default;
        [SerializeField, ReadOnly] private int min = 0;

        public float Current => current;
        public float Min => min;
        public float Max => max;


        public event UnityAction<int, ISource> OnResourceLost;
        public event UnityAction<int, ISource> OnResourceGained;
        public event UnityAction<int, int> OnChanged;
        public event UnityAction OnFillValueChanged;
        public event UnityAction OnEmpty;

        public Resource(int _max, int _min = 0)
        {
            this.max = _max;
            this.min = _min;
        }

        public bool MustGain(int _amount, ISource _source)
        {
            if ((max - current) < _amount) { return false; }

            Gain(_amount, _source);
            return true;
        }

        public void Gain(int _amount, ISource _source)
        {
            int prev = current;
            current += _amount;
            current = Mathf.Clamp(current, min, max);

            if (current != prev)
            {
                OnResourceGained?.Invoke(current - prev, _source);
                OnChanged?.Invoke(prev, current);
                OnFillValueChanged?.Invoke();
            }
        }

        public bool MustLose(int _amount, ISource _source)
        {
            if (current < _amount) { return false; }

            Lose(_amount, _source);
            return true;
        }

        public void Lose(int _amount, ISource _source)
        {
            int prev = current;
            current -= _amount;
            current = Mathf.Clamp(current, min, max);

            if (current != prev)
            {
                OnResourceLost?.Invoke(prev - current, _source);
                OnChanged?.Invoke(prev, current);
                OnFillValueChanged?.Invoke();
            }

            if (current == min)
            {
                OnEmpty?.Invoke();
            }
        }

        public void Fill(ISource _source)
        {
            Gain(max - current, _source);
        }

        public void Empty(ISource _source)
        {
            Lose(current, _source);
        }

        public void SetCurrent(int _amount)
        {
            current = (int)_amount;
            OnFillValueChanged?.Invoke();
        }

        public void SetMin(int _amount)
        {
            min = (int)_amount;
            OnFillValueChanged?.Invoke();
        }

        public void SetMax(int _amount)
        {
            max = (int)_amount;
            OnFillValueChanged?.Invoke();
        }
    }
}