using System;
using DOTweenHelper.Runtime.Sequences;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    [Serializable]
    public class ButtonBuyUpgrade
    {
        public Button Button;
        [SerializeField] private TMP_Text _textLevel;
        [SerializeField] private TMP_Text _textPrice;
        [SerializeField] private DOTweenSequenceController _idleTweenController;
        [SerializeField] private DOTweenSequenceController _failTweenController;

        public void SetText(int level, int maxLevel, float price)
        {
            if (level == maxLevel)
            {
                _textLevel.text = "Max";
                _textPrice.text = "";
            }
            else
            {
                _textLevel.text = $"level {level}";
                _textPrice.text = price.ToString("N0");
            }
        }
            

        public void PlayIdleAnimation()
        {
            if (_failTweenController != null)
                _failTweenController.Complete(true);
            if (_idleTweenController != null)
                _idleTweenController.Play();
        }

        public void PlayFailAnimation()
        {
            if (_idleTweenController != null)
                _idleTweenController.Pause();
            if (_failTweenController != null)
            {
                _failTweenController.OnComplete += CompleteFailAnimationHandler;
                _failTweenController.Play();
            }
                
        }

        private void CompleteFailAnimationHandler()
        {
            _failTweenController.OnComplete -= CompleteFailAnimationHandler;
            PlayIdleAnimation();
        }
    }
}