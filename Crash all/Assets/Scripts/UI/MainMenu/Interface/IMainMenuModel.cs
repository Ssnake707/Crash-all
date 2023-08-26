namespace UI.MainMenu.Interface
{
    public interface IMainMenuModel
    {
        void SetMainMenuAdapter(MainMenuAdapter adapter);
        void SetRotatingSpeed(int levelRotatingSpeed, int maxLevelRotatingSpeed, bool isEffects);
        void SetSizeWeapon(int levelSizeWeapon, int maxLevelSizeWeapon, bool isEffects);
    }
}