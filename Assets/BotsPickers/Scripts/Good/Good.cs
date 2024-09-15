using System;
using UnityEngine;

namespace BotsPickers
{
    public class Good : MonoBehaviour, ITargeted
    {
        [SerializeField] private int _reward = 1;

        private bool _isBusy = false;

        public bool IsBusy => _isBusy;

        public event Action<Good> Collected;

        public void OnBook()
        {
            _isBusy = true;
        }

        public int GiveReward()
        {
            Collected?.Invoke(this);
            return _reward;
        }

        public void ResetParameters()
        {
            _isBusy = false;
        }
    }
}
