using UI.WindowController.Interface;
using UnityEngine;

namespace UI.WindowController
{
    public class WindowsController : MonoBehaviour, IWindowsController
    {
        [SerializeField] private BaseWindow _windowMainMenu;
        [SerializeField] private BaseWindow _windowGameplayMenu;
        [SerializeField] private BaseWindow _windowGeneralMenu;
        [SerializeField] private BaseWindow _windowWin;

        private BaseWindow _currentWindow;
        private BasePopUpWindow _currentPopUpWindow;

        private void Awake()
        {
            _windowMainMenu.SetWindowController(this);
            _windowGameplayMenu.SetWindowController(this);
            _windowGeneralMenu.SetWindowController(this);
        }

        public void ShowWindow(WindowType windowType)
        {
            if (_currentWindow != null) _currentWindow.Hide();
            if (_currentPopUpWindow != null) _currentPopUpWindow.Hide();

            switch (windowType)
            {
                case WindowType.MainMenu:
                    _currentWindow = _windowMainMenu;
                    break;
                case WindowType.GameplayMenu:
                    _currentWindow = _windowGameplayMenu;
                    break;
                case WindowType.GeneralMenu:
                    _currentWindow = _windowGeneralMenu;
                    break;
                case WindowType.WinMenu:
                    _currentWindow = _windowWin;
                    break;
            }
            
            _currentWindow.Show();
        }

        public void ShowPopUpWindow(PopUpWindowType windowType)
        {
            if (_currentPopUpWindow != null) _currentPopUpWindow.Hide();
            switch (windowType)
            {
            }
            
            _currentPopUpWindow.Show();
        }
    }
}