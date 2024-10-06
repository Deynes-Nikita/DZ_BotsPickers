using UnityEngine;

namespace BotsPickers
{
    [RequireComponent(typeof(TruckMovement))]
    [RequireComponent(typeof(GoodsHandler))]
    public class Truck : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance = 3.5f;
        [SerializeField] private SuperMarket _superMarket;

        private TruckMovement _movement;
        private GoodsHandler _handler;

        private Good _good;

        private bool _isBusy = false;
        private bool _isGoodAvailable = false;

        public bool IsBusy => _isBusy;

        private void Awake()
        {
            _movement = GetComponent<TruckMovement>();
            _handler = GetComponent<GoodsHandler>();
        }

        private void OnEnable()
        {
            _movement.Arrived += OnHandlingGood;
        }

        private void OnDisable()
        {
            _movement.Arrived += OnHandlingGood;
        }

        public void SetTargetSuperMarket(SuperMarket targetSuperMarket)
        {
            _superMarket = targetSuperMarket;
        }

        public void GetTask(Good good)
        {
            if (good == null)
                return;

            _good = good;

            _isBusy = true;

            StartMove(_good.transform.position, _good);
        }

        private void StartMove(Vector3 targetPosition, ITargeted targeted)
        {
            _movement.OnMove(targetPosition, _interactionDistance, targeted);
        }

        private void StopMove()
        {
            _movement.StopMoving();
        }

        private void ReturnToSuperMarket()
        {
            StartMove(_superMarket.ReceivingPoint, _superMarket);
        }

        private void ResetTask()
        {
            StopMove();
            _isBusy = false;
        }

        private void OnHandlingGood()
        {
            if (_isGoodAvailable)
            {
                _handler.Drop(_superMarket);
                _isGoodAvailable = false;
                _isBusy = false;
            }
            else
            {
                if (_handler.TryPickup(_good, _interactionDistance))
                {
                    ReturnToSuperMarket();
                    _isGoodAvailable = true;
                }
                else
                {
                    ResetTask();
                }
            }
        }
    }
}