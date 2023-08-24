using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class DataPlayers
    {
        public float Coins;
        public int LevelRotatingSpeed;
        public int LevelSizeWeapon;

        public event Action OnChangeCoins;
        public event Action OnChangeLevelRotateOrSize;

        public void AddCoins(float amount)
        {
            Coins += amount;
            OnChangeCoins?.Invoke();
        }

        public void AddLevelRotatingSpeed(int amount)
        {
            LevelRotatingSpeed += amount;
            OnChangeLevelRotateOrSize?.Invoke();
        }

        public void AddLevelSizeWeapon(int amount)
        {
            LevelSizeWeapon += amount;
            OnChangeLevelRotateOrSize?.Invoke();
        }
    }
}