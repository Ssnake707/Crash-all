namespace UI.WindowController.Interface
{
    public interface IWindowsController
    {
        void ShowWindow(WindowType windowType);
        void ShowPopUpWindow(PopUpWindowType windowType);
    }
}