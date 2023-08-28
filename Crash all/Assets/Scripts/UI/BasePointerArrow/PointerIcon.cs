using System;
using DG.Tweening;
using UI.BasePointerArrow.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BasePointerArrow
{
    public class PointerIcon : MonoBehaviour, IPointerIcon
    {
        public RotationForPlane RotationForPlane => _rotationForPlane;
        [SerializeField] private RotationForPlane _rotationForPlane;
        [SerializeField] private Image _pointerImage;
        [SerializeField, Range(0f, 1f)] private float _smooth = .9f;
       
        private bool _isShow = false;
        private Vector3 _targetPosition;

        public bool IsShow { get; }

        public void Show(bool isShow, Action onComplete = null)
        {
            if (isShow == _isShow) return;

            _isShow = isShow;

            if (_isShow)
                transform.DOScale(Vector3.one, .5f).SetAutoKill(true).SetLink(this.gameObject)
                    .OnComplete(() => onComplete?.Invoke());
            else
                transform.DOScale(Vector3.zero, .5f).OnComplete(() => _isShow = false).SetAutoKill(true)
                    .SetLink(this.gameObject).OnComplete(() => onComplete?.Invoke());
        }

        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            if (!_isShow) return;

            _targetPosition = position;
            transform.rotation = rotation;
        }

        public void DestroyPointerIcon()
        {
            if (!_isShow)
            {
                Destroy(this.gameObject);
                return;
            }

            Show(false, () => Destroy(this.gameObject));
        }

        private void Awake()
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.zero;
        }

        private void LateUpdate()
        {
            if (!_isShow) return;

            transform.position = Vector3.Lerp(transform.position, _targetPosition, _smooth);
        }
    }
}