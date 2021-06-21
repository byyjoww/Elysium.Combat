using System.Collections.Generic;
using System.Linq;

namespace Elysium.Combat
{
    public class DamageRecord
    {
        public Dictionary<IDamageDealer, int> DamageDealers;

        public DamageRecord() => Reset();

        public void Add(IDamageDealer _dealer, int _amount)
        {
            if (DamageDealers.ContainsKey(_dealer))
            {
                DamageDealers[_dealer] += _amount;
                return;
            }

            DamageDealers.Add(_dealer, _amount);
        }

        public void Reset()
        {
            DamageDealers = new Dictionary<IDamageDealer, int>();
        }

        public IDamageDealer GetKiller()
        {
            var allDamagers = DamageDealers.Where(x => x.Key != null).OrderBy(x => x.Value).ToArray();            
            return allDamagers.Length < 1 ? null : allDamagers[0].Key;
        }
    }
}