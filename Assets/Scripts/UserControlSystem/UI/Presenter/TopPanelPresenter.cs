using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using System;
using Abstractions;

public sealed class TopPanelPresenter : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private Button _menuButton;
    [SerializeField] private GameObject _menuGo;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private int _playerFactionId; 
    [Inject]
    private void Init(ITimeModel timeModel, IShowFactionMoney showFaction)
    {
        timeModel.GameTime.Subscribe(seconds =>
        {
            var t = TimeSpan.FromSeconds(seconds);
            _inputField.text = $"{t.Minutes:D2}:{t.Seconds:D2}";
        });

        _menuButton.OnClickAsObservable().Subscribe(_ => _menuGo.SetActive(true));

        _menuGo.ObserveEveryValueChanged(t => t.activeSelf).Subscribe(isMinuOpen =>
        {
            Time.timeScale = Convert.ToInt32(!isMinuOpen);
        });

        showFaction.FactionMoney.ObserveReplace().Subscribe(replaceEvent =>
        {
            if(replaceEvent.Key== _playerFactionId)
            {
                _moneyText.text ="$"+ replaceEvent.NewValue;
            }
        }
        );
    }
}
