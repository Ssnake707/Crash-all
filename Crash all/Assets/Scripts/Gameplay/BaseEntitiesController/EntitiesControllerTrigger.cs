using UnityEngine;

namespace Gameplay.BaseEntitiesController
{
    public class EntitiesControllerTrigger : MonoBehaviour
    {
        [SerializeField] private EntitiesController _entitiesController;

        private void OnTriggerEnter(Collider other) => 
            _entitiesController.TriggerEnter(other);
    }
}