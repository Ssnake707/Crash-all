using UnityEngine;

namespace BasePlayer
{
    public abstract class PlayerMediator : MonoBehaviour
    {
        public abstract bool CanMove { get; }
    }
}