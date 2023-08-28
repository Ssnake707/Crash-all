using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.BreakdownSystem.PoolParticleSystem
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ReturnToPool : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private IObjectPool<ParticleSystem> _pool;

        public void SetPool(IObjectPool<ParticleSystem> pool) => 
            _pool = pool;

        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }
        
        void OnParticleSystemStopped() => 
            _pool.Release(_particleSystem);
    }
}