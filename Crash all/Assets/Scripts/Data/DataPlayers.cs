using System;

namespace Data
{
    [Serializable]
    public class DataPlayers
    {
        public float Coins;

        public event Action OnChangeCoins;

        public void AddCoins(float amount)
        {
            Coins += amount;
            OnChangeCoins?.Invoke();
        }
    }
}