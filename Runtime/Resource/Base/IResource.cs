using UnityEngine.Events;

namespace Elysium.Combat
{
    public interface IResource
    {
        float Current { get; set; }
        float Max { get; set; }

        event UnityAction<int, int> OnChanged;
        event UnityAction OnEmpty;
        event UnityAction OnFillValueChanged;
        event UnityAction<int> OnResourceGained;
        event UnityAction<int> OnResourceLost;
        
        void Gain(int _amount);
        void Lose(int _amount);

        void Fill();
        void Empty();        
    }
}