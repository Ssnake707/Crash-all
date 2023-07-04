using DG.Tweening;
using MyTools.QueueСontroller.QueueСontroller.Scripts;
using UnityEngine;

namespace MyTools.QueueСontroller.Info.Scripts
{
    public class QueueTest : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabCube;
        [SerializeField] private Transform _spawnPosition;
        private QueueController _queueController;

        private void Awake() => 
            _queueController = GetComponent<QueueController>();

        float time = 0;

        private void Update()
        {
            if (!_queueController.HasQueuePlace()) return;
            time += Time.deltaTime;
            if (time < 1f) return;
            time = 0f;

            GameObject g = Instantiate(_prefabCube, _spawnPosition.position, Quaternion.identity);
            IItemQueue item = g.GetComponent<IItemQueue>();

            (Vector3 position, int index) result = _queueController.GetFreePositionQueue();
            item.Move(result.position);
            _queueController.AddInQueue(item, result.index);
            DOTween.Sequence().AppendInterval(7f).OnComplete(() =>
            {
                DataQueue data = _queueController.GetItem();
                Destroy(g);
            });
        }
    }
}