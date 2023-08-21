using UI.WindowController;
using UnityEngine;

namespace UI.MainMenu
{
    public class MainMenuView : BaseWindow
    {
        private void StartGame() => 
            WindowsController.ShowWindow(WindowType.GameplayMenu);

        private void Update()
        {
            if (!IsShow) return;
            float horizontal = SimpleInput.GetAxis("Horizontal");
            float vertical = SimpleInput.GetAxis("Vertical");
            if (Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0f) 
                StartGame();
        }
    }
}