using System.Collections.Generic;
using UnityEngine;

namespace BotsPickers
{
    [RequireComponent(typeof(Scanner))]
    [RequireComponent(typeof(Counter))]
    [RequireComponent(typeof(TruckSpawner))]
    public class SuperMarket : MonoBehaviour, ITargeted
    {
        [SerializeField] private int _truckPrice = 3;
        [SerializeField] private List<Truck> _trucks = new List<Truck>();
        [SerializeField] private Transform _receivingPoint;

        private Scanner _scanner;
        private Counter _counter;
        private TruckSpawner _truckSpawner;
        private List<Good> _currentGoods = new List<Good>();

        public Vector3 ReceivingPoint => _receivingPoint.position;

        private void Awake()
        {
            _scanner = GetComponent<Scanner>();
            _counter = GetComponent<Counter>();
            _truckSpawner = GetComponent<TruckSpawner>();
        }

        private void OnEnable()
        {
            _counter.ScoreRecalculated += OnCreate;
        }

        private void OnDisable()
        {
            _counter.ScoreRecalculated += OnCreate;
        }

        private void Start()
        {
            _counter.GetTruckCount(_trucks.Count);
        }

        private void Update()
        {
            SendForGoods();
        }

        public void AcceptGood(Good good)
        {
            _counter.GetReward(good.GiveReward());
            _currentGoods.Remove(good);
        }

        private bool TrySelectGood(out Good good)
        {
            List<Good> goods = new List<Good>();

            _scanner.Scan(goods);

            if (goods.Count == 0)
            {
                good = null;
                return false;
            }

            foreach (Good item in goods)
            {
                if (item == null || _currentGoods.Contains(item))
                    continue;

                good = item;
                _currentGoods.Add(good);
                return true;
            }

            good = null;
            return false;
        }

        private bool TrySelectTruck(out Truck truck)
        {
            if (_trucks.Count == 0)
            {
                truck = null;
                return false;
            }

            foreach (Truck unit in _trucks)
            {
                if (unit == null)
                    continue;

                if (unit.IsBusy == false)
                {
                    truck = unit;
                    return true;
                }
            }

            truck = null;
            return false;
        }

        private void SendForGoods()
        {
            if (TrySelectTruck(out Truck truck) == false || TrySelectGood(out Good good) == false)
                return;

            truck.GetTask(good);
        }

        private void OnCreate(int score)
        {
            if (score >= _truckPrice)
                CreateTruck();
        }

        private void CreateTruck()
        {
            if (_counter.TryBuy(_truckPrice))
            {
                Truck truck = _truckSpawner.Create();
                truck.SetTargetSuperMarket(this);
                _trucks.Add(truck);

                _counter.GetTruckCount(_trucks.Count);
            }
        }
    }
}
