using System;
using UnityEngine;

namespace BotsPickers
{
    public class Counter : MonoBehaviour
    {
        private int _score = 0;
        private int _truckCount = 0;

        public int Score => _score;

        public event Action <int> ScoreRecalculated;
        public event Action <int> TruckRecalculated;

        public void GetReward(int reward)
        {
            _score += reward;
            ScoreRecalculated?.Invoke(_score);
        }

       public void DeductScore(int price)
        {
            _score -= price;
            ScoreRecalculated?.Invoke(_score);
        }

        public void GetTruckCount(int truckCount)
        {
            _truckCount = truckCount;
            TruckRecalculated?.Invoke(_truckCount);
        }
    }
}