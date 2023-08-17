using UnityEngine;

namespace UI.Gameplay
{
    public class BaseWindow : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        public bool IsShow { get; private set; }
        public virtual void Show()
        {
            IsShow = true;
            _root.gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            IsShow = false;
            _root.gameObject.SetActive(false);
        }
    }
}