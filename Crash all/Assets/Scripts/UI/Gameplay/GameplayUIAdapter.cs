using UI.Gameplay.Interface;

namespace UI.Gameplay
{
    public class GameplayUIAdapter : IGameplayUIAdapter
    {
        private IGameplayView _gameplayView;
        private IGameplayUIModel _gameplayUIModel;

        public GameplayUIAdapter(IGameplayView gameplayView, IGameplayUIModel gameplayUIModel)
        {
            _gameplayView = gameplayView;
            _gameplayUIModel = gameplayUIModel;
            _gameplayView.SetAdapter(this);
            _gameplayUIModel.SetGameplayUIAdapter(this);
        }

        public void DestroyPiece(int totalPieces, int totalDestroyedPieces) =>
            _gameplayView.SetProgressBar((float)totalDestroyedPieces / totalPieces);

        public void LevelComplete() =>
            _gameplayView.LevelComplete();

        public void GameplayViewOnShow() =>
            _gameplayUIModel.StartGame();

        public void GameplayViewOnHide() =>
            _gameplayUIModel.StopGame();

        public void WinMenuOnHide() =>
            _gameplayUIModel.NextLevel();
    }
}