using System.Collections.Generic;
using UnityEngine;

namespace BotsPickers
{
    [RequireComponent(typeof(Scanner))]
    [RequireComponent(typeof(Counter))]
    [RequireComponent(typeof(Creator))]
    public class Base : MonoBehaviour, ITargeted
    {
        [SerializeField] private int _truckPrice = 3;
        [SerializeField] List<Truck> _trucks = new List<Truck>();

        private Scanner _scanner;
        private Counter _counter;
        private Creator _creator;
        private List<Good> _currentGoods = new List<Good>();

        private void Start()
        {
            _scanner = GetComponent<Scanner>();
            _counter = GetComponent<Counter>();
            _creator = GetComponent<Creator>();

            _counter.GetTruckCount(_trucks.Count);
        }

        private void FixedUpdate()
        {
            SendForGoods();

            if (TryCreate(_counter.Score))
                _counter.DeductScore(_truckPrice);
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

        private bool TryCreate(int score)
        {
            if (score < _truckPrice)
                return false;

            _trucks.Add(_creator.Truck(this));
            _counter.GetTruckCount(_trucks.Count);
            return true;
        }
    }
}
