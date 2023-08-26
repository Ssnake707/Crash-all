using System;
using UnityEngine;

namespace Infrastructure
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