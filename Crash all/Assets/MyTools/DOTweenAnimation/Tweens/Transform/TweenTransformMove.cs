using DG.Tweening;
using UnityEngine;

namespace MyTools.DOTweenAnimation.Tweens.Transform
{
    public class TweenTransformMove : BaseTween
    {
        [SerializeField] private UnityEngine.Transform _target;
        [SerializeField] private Vector3 _to;
        [SerializeField] private float _duration;
        protected override Tween CreateTween() => 
            _target.DOMove(_to, _duration);
    }
}