using Services.PersistentProgress;
using TMPro;
using UI.GeneralMenu.Interface;
using UnityEngine;
using Zenject;

namespace UI.GeneralMenu
{
    public class GeneralMenuView : BaseWindow, IGeneralMenuView
    {
        [SerializeField] private TMP_Text _textAmountCoins;
        private IPersistentProgressService _progressService;
        
        [Inject]
        public void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _progressService.Progress.DataPlayers.OnChangeCoins += ChangeCoinsHandler;
            ChangeCoinsHandler();
        }

        private void ChangeCoinsHandler() => 
            _textAmountCoins.text = _progressService.Progress.DataPlayers.Coins.ToString("N0");
    }
}