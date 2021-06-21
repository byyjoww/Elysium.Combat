using UnityEngine.Events;

namespace Elysium.Combat
{
    public interface IResourceController
    {
        float Max { get; }
        float Current { get; }

        event UnityAction<int> OnResourceLost;
        event UnityAction<int> OnResourceGained;
        event UnityAction<int, int> OnChanged;
        event UnityAction OnEmpty;

        bool TryGain(int _amount);
        bool TryLose(int cost);
        bool ForceLose(int _amount);        
        bool Set(int _amount);
        bool Fill();
    }
}