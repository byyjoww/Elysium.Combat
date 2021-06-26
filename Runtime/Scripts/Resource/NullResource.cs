using UnityEngine.Events;

namespace Elysium.Combat
{
    public class NullResource : IResource
    {
        public float Current { get; } = 0f;
        public float Max { get; } = 0f;
        public float Min { get; } = 0f;

        public event UnityAction<int, int> OnChanged;
        public event UnityAction OnEmpty;
        public event UnityAction OnFillValueChanged;
        public event UnityAction<int, ISource> OnResourceGained;
        public event UnityAction<int, ISource> OnResourceLost;

        public bool MustGain(int _amount, ISource _source)
        {
            return false;
        }

        public bool MustLose(int _amount, ISource _source)
        {
            return false;
        }        

        public void Gain(int _amount, ISource _source)
        {
            
        }

        public void Lose(int _amount, ISource _source)
        {
            
        }

        public void Empty(ISource _source)
        {

        }

        public void Fill(ISource _source)
        {

        }

        public void SetCurrent(int _amount)
        {
            
        }

        public void SetMax(int _amount)
        {
           
        }

        public void SetMin(int _amount)
        {
            
        }
    }
}