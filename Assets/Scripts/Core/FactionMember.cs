using Assets.Scripts.Abstractions;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Core
{
    public class FactionMember : MonoBehaviour, IFactionMember
    {
        public int FactionId => _factionId;
        [SerializeField] private int _factionId;

        public static int FactionsCount
        {
            get
            {
                lock (_membersCount)
                {
                    return _membersCount.Count;
                }
            }
        }


        public static List<int> GetFactionList()
        {
            lock (_membersCount)
            {
                return new List<int>(_membersCount.Keys);
            }
        }

        public static IReadOnlyReactiveCollection<Transform> GetMemberFactionList(int fractionId)
        {
            lock (_membersCount)
            {
                return _membersCount[fractionId];
            }
        }

        public static int GetWinner()
        {
            lock (_membersCount)
            {
                return _membersCount.Keys.First();
            }
        }
        private static ReactiveDictionary<int, ReactiveCollection<Transform>> _membersCount = new ReactiveDictionary<int, ReactiveCollection<Transform>>();

        public static IReadOnlyReactiveDictionary<int, ReactiveCollection<Transform>> MembersCount => _membersCount;
        public void SetFaction(int factionId)
        {
            Unregister();
            _factionId = factionId;
            Register();
        }

        private void Awake()
        {
            if (_factionId != 0)
            {
                Register();
            }
        }
        private void OnDestroy()
        {
            Unregister();
        }

        private void Register()
        {
            lock (_membersCount)
            {
                if (!_membersCount.ContainsKey(_factionId))
                {
                    _membersCount.Add(_factionId, new ReactiveCollection<Transform>());
                }
                if (!_membersCount[_factionId].Contains(transform))
                {
                    _membersCount[_factionId].Add(transform);
                }
            }
        }

        private void Unregister()
        {
            if(!_membersCount.ContainsKey(_factionId))
            {
                return;
            }
            lock (_membersCount)
            {
                if (_membersCount[_factionId].Contains(transform))
                {
                    _membersCount[_factionId].Remove(transform);
                }
                if (_membersCount[_factionId].Count == 0)
                {
                    _membersCount.Remove(_factionId);
                }
            }
        }
    }
}

