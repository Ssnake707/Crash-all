using UI.Gameplay;
using UI.Gameplay.Interface;
using UnityEngine;

namespace UI.MainMenu
{
    public class MainMenuController : BaseWindow
    {
        [SerializeField] private GameplayView _gameplayView;
        public IGameplayView GameplayView => _gameplayView;

        private void Awake() => 
            _gameplayView.SetMainMenuController(this);

        private void StartGame()
        {
            Hide();
            _gameplayView.Show();
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