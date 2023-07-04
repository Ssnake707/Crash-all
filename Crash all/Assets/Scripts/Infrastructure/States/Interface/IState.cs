namespace Infrastructure.States.Interface
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}