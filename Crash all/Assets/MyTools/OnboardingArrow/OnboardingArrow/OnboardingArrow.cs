using DG.Tweening;
using UnityEngine;

namespace MyTools.OnboardingArrow.OnboardingArrow
{
    public class OnboardingArrow : MonoBehaviour
    {
        [SerializeField] private float _durationAnimOffset;
        [SerializeField] private float _sizeXMultiplier;
        [SerializeField] private float _posY;

        private Transform _arrow;
        private Transform _startPosition;
        private Transform _endPosition;

        private MeshRenderer _meshRenderer;
        private Material _material;

        private Tween _tween = null;
        private bool _isActive = false;

        private void Awake()
        {
            _arrow = transform;
            _meshRenderer = _arrow.GetComponent<MeshRenderer>();
            _material = _meshRenderer.materials[0];
            StartAnimOffset();
        }

        private void Update()
        {
            if (!_isActive) return;
            _arrow.localScale = new Vector3(Vector3.Distance(_startPosition.position, _endPosition.position) * .1f, _arrow.localScale.y, _arrow.localScale.z);

            Vector3 center = Vector3.Lerp(_startPosition.position, _endPosition.position, .5f);
            center.y = _posY;
            _arrow.position = center;

            _arrow.LookAt(_endPosition);
            Vector3 rotation = _arrow.rotation.eulerAngles;
            rotation.y += 90f;
            rotation.x = 0f;
            _arrow.rotation = Quaternion.Euler(rotation);

            _material.mainTextureScale = new Vector2(_arrow.localScale.x * _sizeXMultiplier, 1f);
        }

        public void Init(Transform start, Transform end)
        {
            _isActive = true;
            this._startPosition = start;
            this._endPosition = end;
            _arrow.gameObject.SetActive(true);
            StartAnimOffset();
        }

        public void Stop()
        {
            if (_tween != null) _tween.Kill();
            _isActive = false;
            _arrow.gameObject.SetActive(false);
            _startPosition = null;
            _endPosition = null;
            Destroy(this.gameObject);
        }

        private void StartAnimOffset()
        {
            if (_tween != null) _tween.Kill();
            _material.mainTextureOffset = Vector2.zero;
            _tween = DOTween.To(() => _material.mainTextureOffset, (x) => _material.mainTextureOffset = x, new Vector2(-10f, 0f), _durationAnimOffset);
            _tween.SetLoops(-1, LoopType.Restart);
            _tween.SetEase(Ease.Linear);
        }
    }
}

