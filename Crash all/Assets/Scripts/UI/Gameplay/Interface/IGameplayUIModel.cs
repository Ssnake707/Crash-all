namespace UI.Gameplay.Interface
{
    public interface IGameplayUIModel
    {
        void SetGameplayUIAdapter(IGameplayUIAdapter adapter);
        void GameplayViewOnShow();
        void GameplayViewOnHide();
    }
}