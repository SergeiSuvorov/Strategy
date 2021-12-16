using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Core
{
    public class EconomicModule : MonoBehaviour, IShowFactionMoney
    {
        public IReadOnlyReactiveDictionary<int, int> FactionMoney => _factionMoney;

        private static ReactiveDictionary<int, int> _factionMoney = new ReactiveDictionary<int, int>();
        private ReactiveProperty<string> property;
        public static void ChangeMoneyCount(int factionId, int MoneyChangeCount)
        {
            _factionMoney[factionId] += MoneyChangeCount;
        }

        public static int GetFactionMoneyCount(int factionId)
        {
            return _factionMoney[factionId];
        }
        private void Start()
        {
           List<int> keys = FactionMember.GetFactionList();
           for(int i=0; i < keys.Count;i++)
           {
                _factionMoney.Add(keys[i],0);
                ChangeMoneyCount(keys[i], 1000);
           }

        }
    }
}
