using System.Collections.Generic;
using UnityEngine;

namespace UI.BasePointerArrow.Interface
{
    public interface IPointerArrowController
    {
        void Init(List<ITargetPointerArrow> targets, Transform playerTransform);
        void AddTarget(ITargetPointerArrow target);
        void Activate(bool isActivate);
        void CleanUp();
    }
}