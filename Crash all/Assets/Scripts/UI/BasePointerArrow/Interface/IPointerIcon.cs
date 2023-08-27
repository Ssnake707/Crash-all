using UnityEngine;

namespace UI.BasePointerArrow.Interface
{
    public interface IPointerIcon
    {
        void Show(bool isShow);
        void SetPosition(Vector3 position, Quaternion rotation);
    }
}