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

        private bool _isMaxLevel = false;

        public void SetText(int level, int maxLevel, float price)
        {
            if (level == maxLevel)
            {
                _textLevel.text = "Max";
                _textPrice.text = "";
                _isMaxLevel = true;
                PauseIdleAnimation();
            }
            else
            {
                _textLevel.text = $"level {level}";
                _textPrice.text = price.ToString("N0");
            }
        }

        public void SetInteractable(bool isInteractable)
        {
            Button.interactable = isInteractable;
            if (_idleTweenController == null) return;
            
            if (isInteractable) 
                PlayIdleAnimation();
            else
                PauseIdleAnimation();
        }

        private void PlayIdleAnimation()
        {
            if (_isMaxLevel) return;
            _idleTweenController.Play();
        }

        private void PauseIdleAnimation() => 
            _idleTweenController.GoTo(0f, false);
    }
}