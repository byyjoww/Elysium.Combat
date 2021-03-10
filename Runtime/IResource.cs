using UnityEngine.Events;

namespace Elysium.Combat
{
    public interface IResource
    {
        float Max { get; }
        float Current { get; }
        bool? PassiveRecoveryEnabled { get; set; }

        bool SetCurrent(int _amount);
        bool GainResource(int _amount);
        bool TryLoseResource(int cost);
        bool ForceLoseResource(int _amount);
        bool PassivelyGainResource(int _amount);
        bool Fill();

        event UnityAction OnFillValueChanged;
        event UnityAction<int> OnResourceLost;
        event UnityAction<int> OnResourceGained;
        event UnityAction<int, int> OnChanged;
    }
}