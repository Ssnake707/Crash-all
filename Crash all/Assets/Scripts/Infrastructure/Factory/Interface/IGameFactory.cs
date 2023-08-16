namespace Infrastructure.Factory.Interface
{
    public interface IGameFactory
    {
        void Init();
        void CleanUp();
        void WarmUp(string nameScene);
    }
}