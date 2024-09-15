using UnityEngine;

namespace BotsPickers
{
    public class Creator : MonoBehaviour
    {
        [SerializeField] private Truck _truckPrefab;

        public Truck Truck(Base targetBase)
        {
            Truck truck = Instantiate(_truckPrefab, transform.position, Quaternion.identity);
            truck.SetParameters(targetBase);

            return truck;
        }
    }
}