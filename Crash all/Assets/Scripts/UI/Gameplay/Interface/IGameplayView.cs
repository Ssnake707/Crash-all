namespace UI.Gameplay.Interface
{
    public interface IGameplayView
    {
        void SetAdapter(IGameplayUIAdapter gameplayUIAdapter);
        void SetProgressBar(float amount);
        void LevelComplete();
    }
}