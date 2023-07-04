using UnityEngine;

namespace MyTools.SafeArea
{
    public class SafeAreaController : MonoBehaviour
    {
        [SerializeField] private bool _igonreBottomSafeZone;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            UpdateSafeArea();
        }

        private void UpdateSafeArea()
        {
            Rect safeArea = Screen.safeArea;
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            if (!_igonreBottomSafeZone)
            {
                _rectTransform.anchorMin = anchorMin;    
            }
            _rectTransform.anchorMax = anchorMax;
        }
    }
}