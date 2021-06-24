using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Elysium.Combat
{
    public class ModificationRecordResource : ResourceDecorator
    {
        private Dictionary<ISource, int> gainRecords;
        private Dictionary<ISource, int> lossRecords;

        public ModificationRecordResource(IResource _resource) : base(_resource)
        {
            ResetGainRecords();
            ResetLossRecords();
        }

        public void Add(int _amount, ISource _source, ref Dictionary<ISource, int> _dictionary)
        {
            if (_source == null)
            {
                Debug.LogError("Attempting to add null source as a resource modifier record");
                return;
            }

            if (_dictionary.ContainsKey(_source))
            {
                _dictionary[_source] += _amount;
            }
            else
            {
                _dictionary.Add(_source, _amount);
            }            
        }

        public IDamageDealer GetHighestContributingAgent(ref Dictionary<ISource, int> _dictionary)
        {
            if (_dictionary.Count < 1) { return null; }

            var modifierAgents = new Dictionary<IDamageDealer, int>();
            foreach (var record in _dictionary)
            {
                if (record.Key.DamageDealer is NullDamageDealer) { continue; }
                if (modifierAgents.ContainsKey(record.Key.DamageDealer))
                {
                    modifierAgents[record.Key.DamageDealer] += record.Value;
                }
                else
                {
                    modifierAgents.Add(record.Key.DamageDealer, record.Value);
                }
            }

            return modifierAgents.OrderBy(x => x.Value).ElementAtOrDefault(0).Key;
        }

        public IElement GetHighestContributingElement(ref Dictionary<ISource, int> _dictionary)
        {
            if (_dictionary.Count < 1) { return null; }

            var modifierAgents = new Dictionary<IElement, int>();
            foreach (var record in _dictionary)
            {
                if (modifierAgents.ContainsKey(record.Key.Element))
                {
                    modifierAgents[record.Key.Element] += record.Value;
                }
                else
                {
                    modifierAgents.Add(record.Key.Element, record.Value);
                }
            }

            return modifierAgents.OrderBy(x => x.Value).ElementAtOrDefault(0).Key;
        }

        public void ResetGainRecords() => gainRecords = new Dictionary<ISource, int>();

        public void ResetLossRecords() => lossRecords = new Dictionary<ISource, int>();

        protected override void TriggerOnResourceGained(int _amount, ISource _source)
        {
            base.TriggerOnResourceGained(_amount, _source);
            Add(_amount, _source, ref gainRecords);
        }

        protected override void TriggerOnResourceLost(int _amount, ISource _source)
        {
            base.TriggerOnResourceLost(_amount, _source);
            Add(_amount, _source, ref lossRecords);
        }
    }

    public static class ModifierRecordingResourceExtension
    {
        public static ModificationRecordResource WithModificationRecord(this IResource _resource)
        {
            return new ModificationRecordResource(_resource);
        }
    }
}