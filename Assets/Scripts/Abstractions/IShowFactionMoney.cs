using System;
using UniRx;

public interface IShowFactionMoney
{
    IReadOnlyReactiveDictionary<int, int> FactionMoney { get; }
}
