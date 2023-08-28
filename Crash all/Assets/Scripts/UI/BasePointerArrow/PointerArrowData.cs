using UI.BasePointerArrow.Interface;

namespace UI.BasePointerArrow
{
    public struct PointerArrowData
    {
        public ITargetPointerArrow Target;
        public IPointerIcon PointerIcon;

        public PointerArrowData(ITargetPointerArrow target, IPointerIcon pointerIcon)
        {
            Target = target;
            PointerIcon = pointerIcon;
        }
    }
}