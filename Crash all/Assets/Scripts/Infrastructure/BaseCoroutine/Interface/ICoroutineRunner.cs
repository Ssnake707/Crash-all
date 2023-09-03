using System.Collections;

namespace Infrastructure.BaseCoroutine.Interface
{
    public interface ICoroutineRunner
    {
        UnityEngine.Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(UnityEngine.Coroutine coroutine);
    }
}