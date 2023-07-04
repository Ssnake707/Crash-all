using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MyTools.Extensions
{
    public static class ScrollRectFocusExtensions
    {
        public static void FocusAtPoint(this ScrollRect scrollView, Vector2 focusPoint, bool isCenter) => 
            scrollView.normalizedPosition = scrollView.CalculateFocusedScrollPosition(focusPoint, isCenter);

        public static void FocusOnItem(this ScrollRect scrollView, RectTransform item, bool isCenter) => 
            scrollView.normalizedPosition = scrollView.CalculateFocusedScrollPosition(item, isCenter);

        public static IEnumerator FocusAtPointCoroutine(this ScrollRect scrollView,
            Vector2 focusPoint, float duration, bool isCenter)
        {
            yield return scrollView
                .LerpToScrollPositionCoroutine(scrollView.CalculateFocusedScrollPosition(focusPoint, isCenter),
                duration);
        }

        public static IEnumerator FocusOnItemCoroutine(this ScrollRect scrollView,
            RectTransform item, float duration, bool isCenter)
        {
            yield return scrollView
                .LerpToScrollPositionCoroutine(scrollView.CalculateFocusedScrollPosition(item, isCenter),
                duration);
        }

        private static IEnumerator LerpToScrollPositionCoroutine(this ScrollRect scrollView,
            Vector2 targetNormalizedPos, float duration)
        {
            Vector2 initialNormalizedPos = scrollView.normalizedPosition;

            float pastTime = 0f;
            while (pastTime < duration)
            {
                scrollView.normalizedPosition = Vector2.LerpUnclamped(initialNormalizedPos, targetNormalizedPos,
                    pastTime / duration);

                pastTime += Time.unscaledDeltaTime;
                yield return null;
            }

            scrollView.normalizedPosition = targetNormalizedPos;
        }

        private static Vector2 CalculateFocusedScrollPosition(this ScrollRect scrollView, RectTransform item, bool isCenter)
        {
            Vector2 itemCenterPoint =
                scrollView.content.InverseTransformPoint(item.transform.TransformPoint(item.rect.center));

            Vector2 contentSizeOffset = scrollView.content.rect.size;
            contentSizeOffset.Scale(scrollView.content.pivot);

            return scrollView.CalculateFocusedScrollPosition(itemCenterPoint + contentSizeOffset, isCenter);
        }

        private static Vector2 CalculateFocusedScrollPosition(this ScrollRect scrollView, Vector2 focusPoint, bool isCenter)
        {
            Vector2 contentSize = scrollView.content.rect.size;
            Vector2 viewportSize = ((RectTransform)scrollView.content.parent).rect.size;
            Vector2 contentScale = scrollView.content.localScale;

            contentSize.Scale(contentScale);
            focusPoint.Scale(contentScale);

            Vector2 scrollPosition = scrollView.normalizedPosition;
            if (scrollView.horizontal && contentSize.x > viewportSize.x)
                scrollPosition.x =
                    Mathf.Clamp01((focusPoint.x - viewportSize.x * (isCenter ? .5f : .7f)) / (contentSize.x - viewportSize.x));
            if (scrollView.vertical && contentSize.y > viewportSize.y)
                scrollPosition.y =
                    Mathf.Clamp01((focusPoint.y - viewportSize.y * (isCenter ? .5f : .7f)) / (contentSize.y - viewportSize.y));

            return scrollPosition;
        }
    }
}