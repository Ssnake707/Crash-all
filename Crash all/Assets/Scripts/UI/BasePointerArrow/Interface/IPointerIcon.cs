using System;
using UnityEngine;

namespace UI.BasePointerArrow.Interface
{
    public interface IPointerIcon
    {
        void Show(bool isShow, Action onComplete = null);
        void SetPosition(Vector3 position, Quaternion rotation);
        void DestroyPointerIcon();
    }
}