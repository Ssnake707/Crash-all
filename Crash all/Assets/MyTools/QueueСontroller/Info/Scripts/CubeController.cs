using MyTools.QueueСontroller.QueueСontroller.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace MyTools.QueueСontroller.Info.Scripts
{
    public class CubeController : MonoBehaviour, IItemQueue
    {
        private NavMeshAgent _agent;

        private void Awake() => 
            _agent = GetComponent<NavMeshAgent>();

        public void Move(Vector3 position) => 
            _agent.destination = position;
    }
}

