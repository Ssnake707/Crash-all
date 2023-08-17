namespace UI.Gameplay.Interface
{
    public interface IGameplayView
    {
        void SetAdapter(IGameplayUIAdapter gameplayUIAdapter);
        void Hide();
        void SetProgressBar(float amount);
    }
}