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

        public void UpdateRotatingSpeed(int levelRotatingSpeed, int maxLevelRotatingSpeed, bool isVFX) => 
            _model.SetRotatingSpeed(levelRotatingSpeed, maxLevelRotatingSpeed, isVFX);
    }
}