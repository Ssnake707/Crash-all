using DG.Tweening;
using UnityEngine;

namespace Tweens.Transform
{
    public class TweenTransformScale : BaseTween
    {
        [SerializeField] private UnityEngine.Transform _target;
        [SerializeField] private Vector3 _to;
        [SerializeField] private float _duration;

        protected override Tween CreateTween() =>
            _target.DOScale(_to, _duration);
    }
}