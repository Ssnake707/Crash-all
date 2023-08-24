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
            if (_idleTweenController != null)
                _idleTweenController.Play();
        }
    }
}