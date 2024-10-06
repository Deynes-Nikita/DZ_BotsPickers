using UnityEngine;

namespace BotsPickers
{
    public class TruckSpawner : MonoBehaviour
    {
        [SerializeField] private Truck _truckPrefab;
        [SerializeField] private Transform _spawnPoint;

        public Truck Create()
        {
            Truck truck = Instantiate(_truckPrefab, _spawnPoint.position, Quaternion.identity);
            return truck;
        }
    }
}