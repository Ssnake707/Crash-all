using UI.MainMenu.Interface;

namespace UI.MainMenu
{
    public class MainMenuAdapter : IMainMenuAdapter
    {
        private readonly IMainMenuView _view;
        private readonly IMainMenuModel _model;

        public MainMenuAdapter(IMainMenuView view, IMainMenuModel model)
        {
            _view = view;
            _model = model;
            _view.SetAdapter(this);
            _model.SetMainMenuAdapter(this);
        }

        public void UpdateRotatingSpeed(int levelRotatingSpeed, int maxLevelRotatingSpeed, bool isEffects) => 
            _model.SetRotatingSpeed(levelRotatingSpeed, maxLevelRotatingSpeed, isEffects);

        public void UpgradeSizeWeapon(int levelSizeWeapon, int maxLevelSizeWeapon, bool isEffects) => 
            _model.SetSizeWeapon(levelSizeWeapon, maxLevelSizeWeapon, isEffects);
    }
}