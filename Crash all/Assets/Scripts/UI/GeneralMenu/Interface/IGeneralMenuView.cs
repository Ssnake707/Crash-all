using Services.PersistentProgress;

namespace UI.GeneralMenu.Interface
{
    public interface IGeneralMenuView
    {
        void Construct(IPersistentProgressService progressService);
    }
}