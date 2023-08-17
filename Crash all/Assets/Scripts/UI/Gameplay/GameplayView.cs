using UI.Gameplay.Interface;

namespace UI.Gameplay
{
    public class GameplayView : BaseWindow, IGameplayView
    {
        private BaseWindow _mainMenuController;
        private IGameplayUIAdapter _gameplayUIAdapter;

        public void SetMainMenuController(BaseWindow mainMenuController) =>
            _mainMenuController = mainMenuController;

        public override void Show()
        {
            base.Show();
        }

        public override void Hide()
        {
            base.Hide();
            _mainMenuController.Show();
        }

        public void SetAdapter(IGameplayUIAdapter gameplayUIAdapter) =>
            _gameplayUIAdapter = gameplayUIAdapter;
    }
}