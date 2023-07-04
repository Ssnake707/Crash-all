using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MyTools.RandomSpawnObject.Info.Scripts
{
    public class ExampleSpawnObject : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(Spawn);
        }

        private void Spawn()
        {
            RandomSpawnObject.RandomSpawnObject r = Instantiate(_prefab, Vector3.zero, Quaternion.identity)
                .GetComponent<RandomSpawnObject.RandomSpawnObject>();
            r.Spawn(Vector3.zero, Vector3.one, () => Debug.Log("Finish spawn"));
            DOTween.Sequence().AppendInterval(5f).OnComplete(() => Destroy(r.gameObject));
        }
    }
}