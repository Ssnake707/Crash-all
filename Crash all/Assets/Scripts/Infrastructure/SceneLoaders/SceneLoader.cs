using System;
using System.Collections;
using Infrastructure.BaseCoroutine;
using Infrastructure.BaseCoroutine.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.SceneLoaders
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunnerWithDestroyEvent _coroutineRunnerWithDestroyEvent;
        
        [Inject]
        public SceneLoader(ICoroutineRunnerWithDestroyEvent coroutineRunnerWithDestroyEvent) => 
            _coroutineRunnerWithDestroyEvent = coroutineRunnerWithDestroyEvent;

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunnerWithDestroyEvent.StartCoroutine(LoadScene(name, onLoaded));

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }
      
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;
            
            onLoaded?.Invoke();
        }
    }
}