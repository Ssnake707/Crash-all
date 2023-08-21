using UnityEngine;
using UnityEngine.UI;

namespace UI.WinMenu
{
    public class WinMenuView : BaseWindow
    {
        public Button ButtonContinue => _buttonContinue;
        
        [SerializeField] private Button _buttonContinue;
    }
}