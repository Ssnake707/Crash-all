using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyTools.QueueСontroller.QueueСontroller.Scripts
{
    public class QueueController : MonoBehaviour
    {
        [Serializable]
        public struct PositionQueue
        {
            public Transform Transform;
            public bool IsBusy;
        }

        [SerializeField] private PositionQueue[] _positionQueue;

        private List<DataQueue> _queueItems = new List<DataQueue>();

        public bool HasQueuePlace()
        {
            for (int i = 0; i < _positionQueue.Length; i++)
            {
                if (_positionQueue[i].IsBusy) continue;
                return true;
            }

            return false;
        }

        public DataQueue GetItem()
        {
            if (_queueItems.Count <= 0) return null;
            DataQueue result = _queueItems[0];
            _positionQueue[_queueItems[0].IndexPlace].IsBusy = false;
            _queueItems.RemoveAt(0);
            ReplaceQueue(0, 0);
            return result;
        }

        private void ReplaceQueue(int indexItem, int indexQueue)
        {
            if (indexItem >= _queueItems.Count) return;
            if (indexQueue >= _positionQueue.Length) return;

            int _newIndexQueue = indexQueue;
            for (int i = indexQueue; i < _positionQueue.Length; i++)
            {
                _newIndexQueue = i + 1;
                if (_positionQueue[i].IsBusy) continue;
                _queueItems[indexItem].Item.Move(_positionQueue[i].Transform.position);
                _positionQueue[_queueItems[indexItem].IndexPlace].IsBusy = false;
                _queueItems[indexItem].IndexPlace = i;
                _positionQueue[i].IsBusy = true;
                break;
            }

            ReplaceQueue(indexItem + 1, _newIndexQueue);
        }

        public (Vector3 position, int index) GetFreePositionQueue()
        {
            (Vector3 position, int index) result;
            for (int i = 0; i < _positionQueue.Length; i++)
            {
                if (_positionQueue[i].IsBusy) continue;
                result.position = _positionQueue[i].Transform.position;
                result.index = i;
                _positionQueue[i].IsBusy = true;
                return result;
            }

            result.index = -1;
            result.position = Vector3.zero;
            return result;
        }

        public void AddInQueue(IItemQueue item, int indexPlace)
        {
            DataQueue itemData = new DataQueue(item, indexPlace);
            _queueItems.Add(itemData);
        }
    }
}