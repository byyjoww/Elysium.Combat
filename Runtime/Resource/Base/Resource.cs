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

        public float Current 
        { 
            get => current; 
            set 
            {
                current = (int)value;
                OnFillValueChanged?.Invoke();
            }
        }
        public float Max 
        { 
            get => max;
            set 
            {
                max = (int)value;
                OnFillValueChanged?.Invoke();
            }
        }


        public event UnityAction<int> OnResourceLost;
        public event UnityAction<int> OnResourceGained;
        public event UnityAction<int, int> OnChanged;
        public event UnityAction OnFillValueChanged;
        public event UnityAction OnEmpty;

        public Resource(int _max, int _min = 0)
        {
            this.max = _max;
            this.min = _min;
        }

        public void Gain(int _amount)
        {
            int prev = current;
            current += _amount;
            current = Mathf.Clamp(current, min, max);

            if (current != prev)
            {
                OnChanged?.Invoke(prev, current);
                OnFillValueChanged?.Invoke();
            }
        }

        public void Lose(int _amount)
        {
            int prev = current;
            current -= _amount;
            current = Mathf.Clamp(current, min, max);

            if (current != prev)
            {
                OnChanged?.Invoke(prev, current);
                OnFillValueChanged?.Invoke();
            }

            if (current == min)
            {
                OnEmpty?.Invoke();
            }
        }

        public void Fill()
        {
            Gain(max - current);
        }

        public void Empty()
        {
            Lose(current);
        }
    }
}