using UI.WindowController.Interface;
using UnityEngine;

namespace UI
{
    public class BaseWindow : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        protected IWindowsController WindowsController;
        public bool IsShow { get; private set; }

        public void SetWindowController(IWindowsController windowsController) => WindowsController = windowsController;

        public virtual void Show()
        {
            IsShow = true;
            _root.gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            IsShow = false;
            _root.gameObject.SetActive(false);
        }
    }
}