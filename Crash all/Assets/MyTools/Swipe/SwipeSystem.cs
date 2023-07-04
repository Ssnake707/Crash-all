using System;
using UnityEngine;

namespace MyTools.Swipe
{
    public class SwipeSystem
    {
        public event Action OnSwipeLeft;
        public event Action OnSwipeRight;

        private readonly float _minSwipeDistX = 100f;
        private Vector2 _startPosTouch;
        private bool _isSwipe = false;
        
        public void Update()
        {
            if (Input.touchCount <= 0) return;
            Touch touch = Input.touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startPosTouch = touch.position;
                    break;
                case TouchPhase.Moved:
                    float swipeDistHorizontal =
                        (new Vector3(touch.position.x, 0, 0) -
                         new Vector3(_startPosTouch.x, 0, 0)).magnitude;
                    if (swipeDistHorizontal > _minSwipeDistX)
                    {
                        float swipeValue = Mathf.Sign(touch.position.x - _startPosTouch.x);
                        if (swipeValue > 0 && !_isSwipe) //to left swipe
                        {
                            _isSwipe = true;
                            OnSwipeLeft?.Invoke();
                        }
                        else if (swipeValue < 0 && !_isSwipe) //to right swipe
                        {
                            _isSwipe = true;
                            OnSwipeRight?.Invoke();
                            
                        }
                    }

                    break;
                case TouchPhase.Ended:
                    _isSwipe = false;
                    break;
            }
        }
    }
}