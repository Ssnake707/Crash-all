using Services.PersistentProgress;
using UI.WindowController;
using UnityEngine;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuView : BaseWindow
    {

        [Inject]
        public void Construct(IPersistentProgressService progressService)
        {
            
        }
        
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