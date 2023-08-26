namespace UI.MainMenu.Interface
{
    public interface IMainMenuModel
    {
        void SetMainMenuAdapter(MainMenuAdapter adapter);
        void SetRotatingSpeed(int levelRotatingSpeed, int maxLevelRotatingSpeed, bool isVFX);
    }
}