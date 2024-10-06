using System;
using UnityEngine;

namespace BotsPickers
{
    public class Good : MonoBehaviour, ITargeted
    {
        [SerializeField] private int _reward = 1;

        private bool _isBusy;

        public bool IsBusy => _isBusy;

        public event Action<Good> Collected;

        private void OnEnable()
        {
            ResetParameters();
        }

        public void OnBook()
        {
            _isBusy = true;
        }

        public int GiveReward()
        {
            Collected?.Invoke(this);
            return _reward;
        }

        private void ResetParameters()
        {
            transform.rotation = Quaternion.identity;
            _isBusy = false;
        }
    }
}
