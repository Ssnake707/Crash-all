using System;
using Infrastructure.BaseCoroutine.Interface;
using UnityEngine;

namespace Infrastructure.BaseCoroutine
{
    public class CoroutineRunnerWithDestroyEvent : MonoBehaviour, ICoroutineRunnerWithDestroyEvent
    {
        public event Action OnDestroyEvent;
        private void Awake() => 
            DontDestroyOnLoad(this);

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) OnDestroyEvent?.Invoke();
        }

        private void OnDestroy() => 
            OnDestroyEvent?.Invoke();
    }
}