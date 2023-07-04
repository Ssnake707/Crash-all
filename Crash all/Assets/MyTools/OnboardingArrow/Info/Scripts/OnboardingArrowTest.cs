using UnityEngine;

namespace MyTools.OnboardingArrow.Info.Scripts
{
    public class OnboardingArrowTest : MonoBehaviour
    {
        [SerializeField] private Transform _object1;
        [SerializeField] private Transform _object2;
        [SerializeField] private GameObject _arrowPrefab;

        private void Start()
        {
            OnboardingArrow.OnboardingArrow arrow = Instantiate(_arrowPrefab)
                .GetComponent<OnboardingArrow.OnboardingArrow>();
            arrow.Init(_object1, _object2);
        }
    }
}