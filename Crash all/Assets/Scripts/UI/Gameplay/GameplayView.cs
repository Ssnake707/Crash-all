using DG.Tweening;
using UI.Gameplay.Interface;
using UI.WindowController;
using UI.WinMenu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class GameplayView : BaseWindow, IGameplayView
    {
        [SerializeField] private WinMenuView _winMenuView;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private Button _buttonRestart;
        [SerializeField] private float _durationSliderAnim;
        private BaseWindow _mainMenuController;
        private IGameplayUIAdapter _gameplayUIAdapter;
        private Tween _tweenSliderValue = null;

        public override void Show()
        {
            _progressBar.value = 0f;
            _gameplayUIAdapter.GameplayViewOnShow();
            base.Show();
        }

        public void LevelComplete()
        {
            _gameplayUIAdapter.GameplayViewOnHide();
            WindowsController.ShowWindow(WindowType.WinMenu);
        }

        public void ShowWindowMainMenu()
        {
            _gameplayUIAdapter.GameplayViewOnHide();
            WindowsController.ShowWindow(WindowType.MainMenu);
        }

        public void SetAdapter(IGameplayUIAdapter gameplayUIAdapter) =>
            _gameplayUIAdapter = gameplayUIAdapter;

        public void SetProgressBar(float amount)
        {
            if (_tweenSliderValue != null) 
                _tweenSliderValue.Kill();
            
            _tweenSliderValue = _progressBar.DOValue(amount, _durationSliderAnim);
        }

        private void ClickContinueWinMenuHandler()
        {
            WindowsController.ShowWindow(WindowType.MainMenu);
            _gameplayUIAdapter.WinMenuOnHide();
        }

        private void Awake()
        {
            _winMenuView.ButtonContinue.onClick.AddListener(ClickContinueWinMenuHandler);
            _buttonRestart.onClick.AddListener(ClickRestartLevelHandler);
        }

        private void ClickRestartLevelHandler() => 
            _gameplayUIAdapter.RestartLevel();
    }
}