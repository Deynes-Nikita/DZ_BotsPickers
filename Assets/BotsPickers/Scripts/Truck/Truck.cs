using UnityEngine;

namespace BotsPickers
{
    [RequireComponent(typeof(TruckMovement))]
    [RequireComponent(typeof(GoodsHandler))]
    public class Truck : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance = 3.5f;
        [SerializeField] private Base _base;

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

        public void SetParameters(Base targetBase)
        {
            _base = targetBase;
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

        private void ReturnToBase()
        {
            StartMove(_base.transform.position, _base);
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
                _handler.Drop(_base);
                _isGoodAvailable = false;
                _isBusy = false;
            }
            else
            {
                if (_handler.TryPickup(_good, _interactionDistance))
                {
                    ReturnToBase();
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