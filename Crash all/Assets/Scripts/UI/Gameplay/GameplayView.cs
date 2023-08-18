using DG.Tweening;
using UI.Gameplay.Interface;
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

        public void SetMainMenuController(BaseWindow mainMenuController) =>
            _mainMenuController = mainMenuController;

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
            _mainMenuController.Show();
        }

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