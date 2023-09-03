namespace UI.Gameplay.Interface
{
    public interface IGameplayUIAdapter
    {
        void DestroyPiece(int totalPieces, int totalDestroyedPieces);
        void LevelComplete();
        void RestartLevel();
        void GameplayViewOnShow();
        void GameplayViewOnHide();
        void WinMenuOnHide();
    }
}