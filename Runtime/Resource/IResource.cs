using UnityEngine.Events;

namespace Elysium.Combat
{
    public interface IResource
    {
        float Max { get; }
        float Current { get; }
        bool PassiveRecoveryEnabled { get; set; }        

        event UnityAction OnFillValueChanged;
        event UnityAction<int> OnResourceLost;
        event UnityAction<int> OnResourceGained;
        event UnityAction<int, int> OnChanged;
        
        bool TryGain(int _amount);
        bool TryGainPassive(int _amount);
        bool TryLose(int cost);
        bool ForceLose(int _amount);        
        bool Set(int _amount);
        bool Fill();
    }
}