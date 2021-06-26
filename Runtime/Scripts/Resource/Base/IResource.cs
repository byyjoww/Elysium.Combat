using UnityEngine.Events;

namespace Elysium.Combat
{
    public interface IResource
    {
        float Current { get; }
        float Min { get; }
        float Max { get; }

        event UnityAction<int, int> OnChanged;
        event UnityAction OnEmpty;
        event UnityAction OnFillValueChanged;
        event UnityAction<int, ISource> OnResourceGained;
        event UnityAction<int, ISource> OnResourceLost;

        bool MustLose(int _amount, ISource _source);
        bool MustGain(int _amount, ISource _source);

        void Gain(int _amount, ISource _source);
        void Lose(int _amount, ISource _source);

        void Fill(ISource _source);
        void Empty(ISource _source);

        void SetCurrent(int _amount);
        void SetMin(int _amount);
        void SetMax(int _amount);
    }
}