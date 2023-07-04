using Infrastructure.States;

namespace Infrastructure.Factory
{
    public interface ILevel
    {
        void Init();
        void NextState(GameStateMachine stateMachine);
        void Cleanup();
    }
}