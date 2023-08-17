using UI.Gameplay;
using UI.Gameplay.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuController : BaseWindow
    {
        [SerializeField] private GameplayView _gameplayView;
        [SerializeField] private Button _tapToStartButton;
        public IGameplayView GameplayView => _gameplayView;

        private void Awake()
        {
            _gameplayView.SetMainMenuController(this);
            _tapToStartButton.onClick.AddListener(TapToStartHandler);
        }

        private void TapToStartHandler()
        {
            Hide();
            _gameplayView.Show();
        }
    }
}