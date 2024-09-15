using UnityEngine;

namespace BotsPickers
{
    public class GoodsСounter : MonoBehaviour
    {
        private int _score = 0;

        public int Score => _score;

        public void GetReward(int reward)
        {
            _score += reward;
        }

       public void DeductScore(int price)
        {
            _score -= price;
        }
    }
}