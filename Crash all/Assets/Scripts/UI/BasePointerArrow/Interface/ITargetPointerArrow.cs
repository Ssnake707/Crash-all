using UnityEngine;

namespace UI.BasePointerArrow.Interface
{
    public interface ITargetPointerArrow
    {
        bool IsActive { get; }
        Vector3 Position { get; }
        GameObject GameObject { get; }
    }
}