using System.Collections;
using UnityEngine;

namespace Infrastructure
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _curtain;
        [SerializeField] private float _durationFade;

        private float _startAlpha;
        private float _pastTime;
        private Coroutine _coroutine = null;

        private void Awake() =>
            DontDestroyOnLoad(this);

        public void Show() =>
            StartDoFade(1f);

        public void Hide() =>
            StartDoFade(0f);

        private void StartDoFade(float value)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(DoFade(value));
        }

        private IEnumerator DoFade(float value)
        {
            _curtain.blocksRaycasts = value == 1f;
            _pastTime = 0f;
            _startAlpha = _curtain.alpha;
            while (_pastTime < _durationFade)
            {
                float t = _pastTime / _durationFade;
                _curtain.alpha = Mathf.Lerp(_startAlpha, value, t);
                _pastTime += Time.deltaTime;
                yield return null;
            }

            _curtain.alpha = value;
        }
    }
}