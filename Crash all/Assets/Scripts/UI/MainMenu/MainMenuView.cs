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
            UpdateButtonSizeWeapon();
            UpdateButtonRotatingSpeed();
        }

        public void SetAdapter(MainMenuAdapter adapter) => 
            _adapter = adapter;

        private void ClickBuySizeWeaponHandler()
        {
            if (_progressService.Progress.DataPlayers.LevelSizeWeapon 
                == _staticDataService.DataPriceSizeWeapon.MaxLevel) return;
            
            float price = Mathf.RoundToInt(_staticDataService.DataPriceSizeWeapon
                .GetValue(_progressService.Progress.DataPlayers.LevelSizeWeapon + 1));
            
            if (price > _progressService.Progress.DataPlayers.Coins)
            {
                // fail
                _buttonUpgradeSizeWeapon.PlayFailAnimation();
            }
            else
            {
                // succes
                _progressService.Progress.DataPlayers.AddCoins(-price);
                _progressService.Progress.DataPlayers.AddLevelSizeWeapon(1);
                // _adapter.UpgradeSizeWeapon(_progressService.Progress.DataPlayers.LevelSizeWeapon);
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
                _buttonUpgradeRotatingSpeed.PlayFailAnimation();
            }
            else
            {
                // succes
                _progressService.Progress.DataPlayers.AddCoins(-price);
                _progressService.Progress.DataPlayers.AddLevelRotatingSpeed(1);
                // _adapter.UpgradeRotatingSpeed(_progressService.Progress.DataPlayers.LevelSizeWeapon);
                UpdateButtonRotatingSpeed();
            }
        }

        private void UpdateButtonSizeWeapon() =>
            _buttonUpgradeSizeWeapon.SetText(_progressService.Progress.DataPlayers.LevelSizeWeapon,
                _staticDataService.DataPriceSizeWeapon.MaxLevel,
                _staticDataService.DataPriceSizeWeapon
                    .GetValue(_progressService.Progress.DataPlayers.LevelSizeWeapon + 1));

        private void UpdateButtonRotatingSpeed() =>
            _buttonUpgradeRotatingSpeed.SetText(_progressService.Progress.DataPlayers.LevelRotatingSpeed,
                _staticDataService.DataPriceRotatingSpeed.MaxLevel,
                _staticDataService.DataPriceRotatingSpeed
                    .GetValue(_progressService.Progress.DataPlayers.LevelRotatingSpeed + 1));

        private void StartGame() => 
            WindowsController.ShowWindow(WindowType.GameplayMenu);

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