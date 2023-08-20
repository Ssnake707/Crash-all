using DG.Tweening;
using UI.Gameplay.Interface;
using UI.WindowController;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class GameplayView : BaseWindow, IGameplayView
    {
        [SerializeField] private Slider _progressBar;
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

        public override void Hide()
        {
            _gameplayUIAdapter.GameplayViewOnHide();
            base.Hide();
        }

        public void LevelComplete() => 
            WindowsController.ShowWindow(WindowType.MainMenu);

        public void SetAdapter(IGameplayUIAdapter gameplayUIAdapter) =>
            _gameplayUIAdapter = gameplayUIAdapter;

        public void SetProgressBar(float amount)
        {
            if (_tweenSliderValue != null) 
                _tweenSliderValue.Kill();
            
            _tweenSliderValue = _progressBar.DOValue(amount, _durationSliderAnim);
        }
    }
}