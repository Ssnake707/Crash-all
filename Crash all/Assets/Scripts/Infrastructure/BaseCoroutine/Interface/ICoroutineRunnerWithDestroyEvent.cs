using System;

namespace Infrastructure.BaseCoroutine.Interface
{
    public interface ICoroutineRunnerWithDestroyEvent : ICoroutineRunner
    {
        event Action OnDestroyEvent;
    }
}