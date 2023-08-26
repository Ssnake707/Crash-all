using System;
using System.Collections;
using UnityEngine;

namespace Infrastructure
{
    public interface ICoroutineRunnerWithDestroyEvent : ICoroutineRunner
    {
        event Action OnDestroyEvent;
    }

    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
    }
}