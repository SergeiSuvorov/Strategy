using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Abstractions
{
    public interface IFactionMember
    {
        int FactionId { get; }
    }
}