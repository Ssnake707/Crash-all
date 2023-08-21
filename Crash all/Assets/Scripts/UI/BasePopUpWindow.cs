using System.Collections;
using StaticData.UI;
using UI.WindowController.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class BasePopUpWindow : MonoBehaviour
    {
        [Header("Animation settings")] [SerializeField]
        private CanvasGroup _backgroundCanvasGroup;

        [SerializeField] private GameObject _rootContainer;
        [SerializeField] private RectTransform _mainContainer;
        [SerializeField] private StaticDataAnimationPopUpWindow _animationPopUpWindowData;

        [Space(5), Header("Settings")] [SerializeField]
        private Button[] _buttonCloses;
        
        protected IWindowsController WindowsController;
        private Coroutine _coroutineAnimationWindow = null;
        private Coroutine _coroutineAnimationBackground = null;
        
        public void SetWindowController(IWindowsController windowsController) => WindowsController = windowsController;
        
        private void Start() =>
            OnStart();

        protected virtual void OnStart()
        {
            foreach (Button button in _buttonCloses)
            {
                button.onClick.AddListener(Hide);
            }
        }
        
        public virtual void Show()
        {
            StopCoroutineAnimation();
            if (_mainContainer != null)
                _coroutineAnimationWindow = StartCoroutine(AnimationWindow(true));
            if (_backgroundCanvasGroup != null)
                _coroutineAnimationBackground = StartCoroutine(AnimationBackground(true));
        }

        public virtual void Hide()
        {
            StopCoroutineAnimation();
            if (_mainContainer != null)
                _coroutineAnimationWindow = StartCoroutine(AnimationWindow(false));
            if (_backgroundCanvasGroup != null)
                _coroutineAnimationBackground = StartCoroutine(AnimationBackground(false));
        }

        private void StopCoroutineAnimation()
        {
            if (_coroutineAnimationWindow != null)
            {
                StopCoroutine(_coroutineAnimationWindow);
                _coroutineAnimationWindow = null;
            }

            if (_coroutineAnimationBackground != null)
            {
                StopCoroutine(_coroutineAnimationBackground);
                _coroutineAnimationBackground = null;
            }
        }

        private IEnumerator AnimationBackground(bool isOpen)
        {
            if (isOpen)
                _rootContainer.SetActive(true);
            float startValueBackground = _backgroundCanvasGroup.alpha;
            float endValueBackground = isOpen ? 1f : 0f;
            float pastTime = 0f;
            float maxDuration = _animationPopUpWindowData._durationSecondsBackground >
                                _animationPopUpWindowData._durationSecondsMainContainer
                ? _animationPopUpWindowData._durationSecondsBackground
                : _animationPopUpWindowData._durationSecondsMainContainer;
            while (pastTime < maxDuration)
            {
                float tBackground =
                    Mathf.Clamp01(pastTime / _animationPopUpWindowData._durationSecondsBackground);
                _backgroundCanvasGroup.alpha = Mathf.Lerp(startValueBackground, endValueBackground, tBackground);
                pastTime += Time.unscaledDeltaTime;
                yield return null;
            }

            _rootContainer.SetActive(isOpen);
            _backgroundCanvasGroup.alpha = endValueBackground;
        }

        private IEnumerator AnimationWindow(bool isOpen)
        {
            if (isOpen) 
                _rootContainer.SetActive(true);

            Vector3 startScale = _mainContainer.localScale;
            Vector3 endScale = isOpen ? Vector3.one : Vector3.zero;
            AnimationCurve animationCurveContainer = isOpen
                ? _animationPopUpWindowData._animationCurveOpenWindow
                : _animationPopUpWindowData._animationCurveCloseWindow;

            float pastTime = 0f;
            float maxDuration = _animationPopUpWindowData._durationSecondsBackground >
                                _animationPopUpWindowData._durationSecondsMainContainer
                ? _animationPopUpWindowData._durationSecondsBackground
                : _animationPopUpWindowData._durationSecondsMainContainer;
            while (pastTime < maxDuration)
            {
                float tMainContainer =
                    Mathf.Clamp01(pastTime / _animationPopUpWindowData._durationSecondsMainContainer);
                _mainContainer.localScale = Vector3.LerpUnclamped(startScale, endScale,
                    animationCurveContainer.Evaluate(tMainContainer));
                pastTime += Time.unscaledDeltaTime;
                yield return null;
            }

            _mainContainer.localScale = endScale;
            _rootContainer.SetActive(isOpen);
        }

#if UNITY_EDITOR
        [NaughtyAttributes.Button("Show")]
        public void EditorShowWindow()
        {
            if (_backgroundCanvasGroup != null)
                _backgroundCanvasGroup.alpha = 1f;
            if (_mainContainer != null)
                _mainContainer.localScale = Vector3.one;
            _rootContainer.SetActive(true);
        }

        [NaughtyAttributes.Button("Hide")]
        public void EditorHideWindow()
        {
            if (_backgroundCanvasGroup != null)
                _backgroundCanvasGroup.alpha = 0f;
            if (_mainContainer != null)
                _mainContainer.localScale = Vector3.zero;
            _rootContainer.SetActive(false);
        }
#endif
    }
}