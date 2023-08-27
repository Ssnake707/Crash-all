using System.Collections.Generic;
using UI.BasePointerArrow.Interface;
using UnityEngine;

namespace UI.BasePointerArrow
{
    public class PointerArrowController : MonoBehaviour, IPointerArrowController
    {
        [SerializeField] private GameObject _pointerArrowPrefab;
        [SerializeField] private Canvas _canvas;
        
        private Camera _camera;
        private bool _isActive;
        private Transform _playerTransform;
        private List<IPointerIcon> _pointerIcons;
        private List<PointerArrowData> _targetsPointerArrow = new List<PointerArrowData>();

        public void Init(List<ITargetPointerArrow> targets, Transform playerTransform)
        {
            _playerTransform = playerTransform;
            foreach (ITargetPointerArrow target in targets) 
                AddTarget(target);
        }

        public void AddTarget(ITargetPointerArrow target) =>
            _targetsPointerArrow.Add(new PointerArrowData(target,
                Instantiate(_pointerArrowPrefab, _canvas.transform).GetComponent<IPointerIcon>()));

        public void Activate(bool isActivate)
        {
            _isActive = isActivate;
            foreach (IPointerIcon pointerIcon in _pointerIcons) 
                pointerIcon.Show(_isActive);
        }

        private void Awake() => 
            _camera = Camera.main;

        private void FixedUpdate()
        {
            if (!_isActive) return;

            // Left, Right, Down, Up
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);
            foreach (PointerArrowData target in _targetsPointerArrow)
            {
                if (!target.Target.IsActive)
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

                target.PointerIcon.Show(toTarget.magnitude > rayMinDistance);
                target.PointerIcon.SetPosition(position, rotation);
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