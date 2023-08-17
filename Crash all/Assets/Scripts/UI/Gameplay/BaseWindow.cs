using UnityEngine;

namespace UI.Gameplay
{
    public class BaseWindow : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        public virtual void Show()
        {
            _root.gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            _root.gameObject.SetActive(false);
        }
    }
}