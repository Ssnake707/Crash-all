namespace UI.Gameplay.Interface
{
    public interface IGameplayUIAdapter
    {
        void DestroyPiece(int totalPieces, int totalDestroyedPieces);
        void LevelComplete();
        void GameplayViewOnShow();
        void GameplayViewOnHide();
    }
}