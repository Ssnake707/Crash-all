using Services.PersistentProgress;
using Services.StaticData;
using UI.MainMenu.Interface;
using UI.WindowController;
using UnityEngine;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuView : BaseWindow, IMainMenuView
    {
        [SerializeField] private ButtonBuyUpgrade _buttonUpgradeRotatingSpeed;
        [SerializeField] private ButtonBuyUpgrade _buttonUpgradeSizeWeapon;
        private IPersistentProgressService _progressService;
        private IStaticDataService _staticDataService;
        private MainMenuAdapter _adapter;

        [Inject]
        public void Construct(IPersistentProgressService progressService, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _progressService = progressService;
            _buttonUpgradeRotatingSpeed.Button.onClick.AddListener(ClickBuyRotatingSpeedHandler);
            _buttonUpgradeSizeWeapon.Button.onClick.AddListener(ClickBuySizeWeaponHandler);
            _progressService.Progress.DataPlayers.OnChangeCoins += ChangeCoinsHandler;
            UpdateButtonSizeWeapon();
            UpdateButtonRotatingSpeed();
        }

        public void SetAdapter(MainMenuAdapter adapter)
        {
            _adapter = adapter;
            _adapter.UpdateRotatingSpeed(_progressService.Progress.DataPlayers.LevelRotatingSpeed,
                _staticDataService.DataPriceRotatingSpeed.MaxLevel, false);
        }

        private void ClickBuySizeWeaponHandler()
        {
            if (_progressService.Progress.DataPlayers.LevelSizeWeapon 
                == _staticDataService.DataPriceSizeWeapon.MaxLevel) return;
            
            float price = Mathf.RoundToInt(_staticDataService.DataPriceSizeWeapon
                .GetValue(_progressService.Progress.DataPlayers.LevelSizeWeapon + 1));
            
            if (price > _progressService.Progress.DataPlayers.Coins)
            {
                // fail
            }
            else
            {
                // succes
                _progressService.Progress.DataPlayers.AddCoins(-price);
                _progressService.Progress.DataPlayers.AddLevelSizeWeapon(1);
                // _adapter.UpgradeSizeWeapon(_progressService.Progress.DataPlayers.LevelSizeWeapon, _staticDataService.DataPriceSizeWeapon.MaxLevel);
                UpdateButtonSizeWeapon();
            }
        }

        private void ClickBuyRotatingSpeedHandler()
        {
            if (_progressService.Progress.DataPlayers.LevelRotatingSpeed 
                == _staticDataService.DataPriceRotatingSpeed.MaxLevel) return;
            
            float price = Mathf.RoundToInt(_staticDataService.DataPriceRotatingSpeed
                .GetValue(_progressService.Progress.DataPlayers.LevelRotatingSpeed + 1));
            if (price > _progressService.Progress.DataPlayers.Coins)
            {
                // fail
            }
            else
            {
                // succes
                _progressService.Progress.DataPlayers.AddCoins(-price);
                _progressService.Progress.DataPlayers.AddLevelRotatingSpeed(1);
                _adapter.UpdateRotatingSpeed(_progressService.Progress.DataPlayers.LevelRotatingSpeed,
                    _staticDataService.DataPriceRotatingSpeed.MaxLevel, true);
                UpdateButtonRotatingSpeed();
            }
        }

        private void UpdateButtonSizeWeapon()
        {
            float price = _staticDataService.DataPriceSizeWeapon
                .GetValue(_progressService.Progress.DataPlayers.LevelSizeWeapon + 1);
            
            _buttonUpgradeSizeWeapon.SetText(_progressService.Progress.DataPlayers.LevelSizeWeapon,
                _staticDataService.DataPriceSizeWeapon.MaxLevel, price);
            _buttonUpgradeSizeWeapon.SetInteractable(price <= _progressService.Progress.DataPlayers.Coins);
        }

        private void UpdateButtonRotatingSpeed()
        {
            float price = _staticDataService.DataPriceRotatingSpeed
                .GetValue(_progressService.Progress.DataPlayers.LevelRotatingSpeed + 1);
            _buttonUpgradeRotatingSpeed.SetText(_progressService.Progress.DataPlayers.LevelRotatingSpeed,
                _staticDataService.DataPriceRotatingSpeed.MaxLevel, price);
            _buttonUpgradeRotatingSpeed.SetInteractable(price <= _progressService.Progress.DataPlayers.Coins);
        }

        private void StartGame() => 
            WindowsController.ShowWindow(WindowType.GameplayMenu);

        private void ChangeCoinsHandler()
        {
            UpdateButtonSizeWeapon();
            UpdateButtonRotatingSpeed();
        }
        
        private void Update()
        {
            if (!IsShow) return;
            float horizontal = SimpleInput.GetAxis("Horizontal");
            float vertical = SimpleInput.GetAxis("Vertical");
            if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0f) 
                StartGame();
        }
    }
}