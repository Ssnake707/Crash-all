using UnityEngine;

namespace StaticData.UI
{
    [CreateAssetMenu(fileName = "AnimationPopUpWindow", menuName = "Static data/UI/Animation pop up window", order = 0)]
    public class StaticDataAnimationPopUpWindow : ScriptableObject
    {
        public float _durationSecondsBackground;
        public float _durationSecondsMainContainer;
        public AnimationCurve _animationCurveOpenWindow;
        public AnimationCurve _animationCurveCloseWindow;
    }
}