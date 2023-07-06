using Infrastructure.States;

namespace Infrastructure.Factory.Interface
{
    public interface ILevel
    {
        void Init();
        void NextState(GameStateMachine stateMachine);
        void Cleanup();
    }
}