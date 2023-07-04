using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace MyTools.RandomSpawnObject.RandomSpawnObject
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class RandomSpawnObject : MonoBehaviour
    {
        [SerializeField] private float _durationAnim;
        [SerializeField] private float _jumpPower;

        [SerializeField, MinMaxSlider(0f, 30f)]
        private Vector2 _minMaxRadius;

        private Tween _currentTween;

        private Vector3 _scale;
        private Rigidbody _rigidBody;
        private Collider _collider;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _scale = transform.localScale;
        }

        public void Spawn(Vector3 position, Vector3 positionDrop, Action onFinish)
        {
            if (_currentTween != null)
            {
                _currentTween.Kill();
                _currentTween = null;
            }

            float sizeCollider = _collider.bounds.size.y;
            _collider.enabled = false;
            _rigidBody.isKinematic = true;
            transform.localScale = Vector3.zero;
            transform.position = position;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(_scale, _durationAnim / 2f));
            Vector2 p = UnityEngine.Random.insideUnitCircle.normalized;
            Vector3 randomPos = positionDrop +
                                ((new Vector3(p.y, 0f, p.x)) *
                                 UnityEngine.Random.Range(_minMaxRadius.x, _minMaxRadius.y));
            randomPos.y = sizeCollider / 2f;

            sequence.Join(transform.DOJump(randomPos, _jumpPower, 1, _durationAnim));
            sequence.OnComplete(() =>
            {
                _collider.enabled = true;
                _rigidBody.isKinematic = false;
                onFinish?.Invoke();
            });
            _currentTween = sequence;
        }

#if UNITY_EDITOR
        [Space(10f), Header("Debug"), SerializeField]
        private bool _isDebug = false;

        private void OnDrawGizmos()
        {
            if (!_isDebug) return;

            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position, Vector3.up, _minMaxRadius.x);
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.up, _minMaxRadius.y);
        }

        [ShowIf("isDebug"), Button("Test spawn coin", enabledMode: EButtonEnableMode.Playmode)]
        public void TestSpawn()
        {
            Spawn(this.transform.position, this.transform.position, null);
        }
#endif
    }
}