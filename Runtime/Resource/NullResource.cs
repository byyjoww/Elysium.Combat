using UnityEngine.Events;

namespace Elysium.Combat
{
    public class NullResource : IResource
    {
        public float Current { get; set; }
        public float Max { get; set; }

        public event UnityAction<int, int> OnChanged;
        public event UnityAction OnEmpty;
        public event UnityAction OnFillValueChanged;
        public event UnityAction<int> OnResourceGained;
        public event UnityAction<int> OnResourceLost;

        public void Empty()
        {
            
        }

        public void Fill()
        {
            
        }

        public void Gain(int _amount)
        {
            
        }

        public void Lose(int _amount)
        {
            
        }
    }
}