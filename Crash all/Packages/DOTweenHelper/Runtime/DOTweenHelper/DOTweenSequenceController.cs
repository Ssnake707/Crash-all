using System;
using DG.Tweening;
using Tweens;
using UnityEngine;

namespace DOTweenHelper
{
    public class DOTweenSequenceController : MonoBehaviour, IDOTweenAnimationController
    {
        public event Action OnComplete;
        public event Action OnKill;
        public event Action OnPlay;
        public event Action OnPause;
        public event Action OnRewind;
        public event Action OnStart;
        public event Action OnStepComplete;
        public event Action OnUpdate;
        
        [SerializeField] private GameObject _linkGameObject;
        [SerializeField] private bool _isPlayOnAwake;
        [SerializeField] private SequenceLoopsSettings _sequenceLoopsSettings;
        [SerializeField] private SequenceUpdateSettings _sequenceUpdateSettings;
        [SerializeField] private BaseTween[] _tweens;
        private Sequence _sequence = null;

        public bool IsPlaying => _sequence?.IsPlaying() ?? false;

        private void Awake()
        {
            if (_isPlayOnAwake)
                Play();
        }

        public void Play()
        {
            if (_sequence == null) CreateSequence();
            _sequence.Play();
        }

        public void Pause()
        {
            if (_sequence == null) return;
            _sequence.Pause();
        }

        public void Complete()
        {
            if (_sequence == null) return;
            _sequence.Complete(true);
        }

        private void CreateSequence()
        {
            _sequence = DOTween.Sequence();
            foreach (BaseTween tween in _tweens)
            {
                switch (tween.AddTypeTween)
                {
                    case AddTypeTween.Append:
                        _sequence.Append(tween.GetTween());
                        break;
                    case AddTypeTween.Join:
                        _sequence.Join(tween.GetTween());
                        break;
                }
            }
            SetSettings();
            SetEvents();
        }

        private void SetSettings()
        {
            _sequence.SetLink(_linkGameObject);
            _sequence.SetAutoKill(false);
            _sequence.SetLoops(_sequenceLoopsSettings.CountLoops, _sequenceLoopsSettings.LoopType);
            _sequence.SetUpdate(_sequenceUpdateSettings.UpdateType, _sequenceUpdateSettings.IgnoreTimeScale);
        }

        private void SetEvents()
        {
            _sequence.OnComplete(() => OnComplete?.Invoke());
            _sequence.OnKill(() => OnKill?.Invoke());
            _sequence.OnPlay(() => OnPlay?.Invoke());
            _sequence.OnPause(() => OnPause?.Invoke());
            _sequence.OnRewind(() => OnRewind?.Invoke());
            _sequence.OnStart(() => OnStart?.Invoke());
            _sequence.OnStepComplete(() => OnStepComplete?.Invoke());
            _sequence.OnUpdate(() => OnUpdate?.Invoke());
        }
    }
}