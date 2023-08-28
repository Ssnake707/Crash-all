using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.BreakdownSystem.PoolParticleSystem
{
    public class PoolParticleSystemHit
    {
        private GameObject _particlePrefab;
        public readonly IObjectPool<ParticleSystem> Pool;

        public PoolParticleSystemHit(GameObject particlePrefab)
        {
            _particlePrefab = particlePrefab;
            Pool = new ObjectPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
                OnDestroyPoolObject, true, 30, 100);
        }

        private ParticleSystem CreatePooledItem()
        {
            GameObject goParticleSystem = Object.Instantiate(_particlePrefab);
            ParticleSystem particleSystem = goParticleSystem.GetComponent<ParticleSystem>();
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            
            ReturnToPool returnToPool = goParticleSystem.AddComponent<ReturnToPool>();
            returnToPool.SetPool(Pool);

            return particleSystem;
        }

        private void OnTakeFromPool(ParticleSystem obj) => 
            obj.gameObject.SetActive(true);

        private void OnReturnedToPool(ParticleSystem obj) => 
            obj.gameObject.SetActive(false);

        private void OnDestroyPoolObject(ParticleSystem obj) => 
            Object.Destroy(obj.gameObject);
    }
}