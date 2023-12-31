namespace UI.Gameplay.Interface
{
    public interface IGameplayUIModel
    {
        void SetGameplayUIAdapter(IGameplayUIAdapter adapter);
        void StartGame();
        void StopGame();
        void NextLevel();
        void RestartGame();
        void ActivateCameraPlayer();
        void ActivateCameraWin();
    }
}