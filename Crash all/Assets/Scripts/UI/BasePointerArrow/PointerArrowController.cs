using System.Collections.Generic;
using UI.BasePointerArrow.Interface;
using UnityEngine;

namespace UI.BasePointerArrow
{
    public class PointerArrowController : MonoBehaviour, IPointerArrowController
    {
        [SerializeField] private GameObject _pointerArrowPrefab;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private bool _isAlwaysShow;

        private readonly List<PointerArrowData> _targetsPointerArrow = new List<PointerArrowData>();
        private readonly Queue<IPointerIcon> _warmUpPointerIcon = new Queue<IPointerIcon>();
        private Camera _camera;
        private bool _isActive;
        private Transform _playerTransform;

        public void WarmUp(int countPointerIcon)
        {
            for (int i = 0; i < countPointerIcon; i++)
                _warmUpPointerIcon.Enqueue(Instantiate(_pointerArrowPrefab, _canvas.transform)
                    .GetComponent<PointerIcon>());
        }
        
        public void Init(List<ITargetPointerArrow> targets, Transform playerTransform)
        {
            _playerTransform = playerTransform;
            foreach (ITargetPointerArrow target in targets) 
                AddTarget(target);
        }

        public void CleanUp()
        {
            foreach (PointerArrowData item in _targetsPointerArrow) 
                item.PointerIcon.DestroyPointerIcon();
            
            _targetsPointerArrow.Clear();
            
            foreach (IPointerIcon item in _warmUpPointerIcon) 
                item.DestroyPointerIcon();
            
            _warmUpPointerIcon.Clear();
        }

        public void AddTarget(ITargetPointerArrow target)
        {
            IPointerIcon pointerIcon = null;
            if (_warmUpPointerIcon.TryDequeue(out IPointerIcon result))
                pointerIcon = result;
            else
                pointerIcon = Instantiate(_pointerArrowPrefab, _canvas.transform)
                    .GetComponent<PointerIcon>();
            
            _targetsPointerArrow.Add(new PointerArrowData(target, pointerIcon));
        }

        public void Activate(bool isActivate)
        {
            _isActive = isActivate;
            foreach (PointerArrowData item in _targetsPointerArrow) 
                item.PointerIcon.Show(_isActive);
        }

        private void Awake() => 
            _camera = Camera.main;

        private void FixedUpdate()
        {
            if (!_isActive) return;
            RemoveNullTargets();
            // Left, Right, Down, Up
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);
            foreach (PointerArrowData target in _targetsPointerArrow)
            {
                if ((target.Target == null || target.Target.Equals(null)) || !target.Target.IsActive)
                {
                    target.PointerIcon.Show(false);
                    continue;
                }

                Vector3 toTarget = target.Target.Position - _playerTransform.position;
                Ray ray = new Ray(_playerTransform.position, toTarget);

                float rayMinDistance = Mathf.Infinity;
                int index = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (!planes[i].Raycast(ray, out float distance)) continue;
                    if (!(distance < rayMinDistance)) continue;
                    
                    rayMinDistance = distance;
                    index = i;
                }

                rayMinDistance = Mathf.Clamp(rayMinDistance, 0, toTarget.magnitude);
                Vector3 worldPosition = ray.GetPoint(rayMinDistance);
                Vector3 position = _camera.WorldToScreenPoint(worldPosition);
                Quaternion rotation = GetIconRotation(index);
                
                target.PointerIcon.Show(_isAlwaysShow || toTarget.magnitude > rayMinDistance);
                target.PointerIcon.SetPosition(position, rotation);
            }
        }

        private void RemoveNullTargets()
        {
            for (int i = _targetsPointerArrow.Count - 1; i >= 0; i--)
            {
                if (_targetsPointerArrow[i].Target != null && !_targetsPointerArrow[i].Target.Equals(null)) continue;

                _targetsPointerArrow[i].PointerIcon.DestroyPointerIcon();
                _targetsPointerArrow.RemoveAt(i);
            }
        }

        private Quaternion GetIconRotation(int planeIndex)
        {
            return planeIndex switch
            {
                0 => Quaternion.Euler(0f, 0f, 90f),
                1 => Quaternion.Euler(0f, 0f, -90f),
                2 => Quaternion.Euler(0f, 0f, 180),
                3 => Quaternion.Euler(0f, 0f, 0f),
                _ => Quaternion.identity
            };
        }
    }
}