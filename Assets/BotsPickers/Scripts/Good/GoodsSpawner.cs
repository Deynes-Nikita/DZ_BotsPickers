using UnityEngine;
using UnityEngine.Pool;

namespace BotsPickers
{
    public class GoodsSpawner : MonoBehaviour
    {
        [SerializeField] private Good _goodPrefab;
        [SerializeField] private float _repeatRate = 3f;
        [SerializeField] private int _maxCountGoods = 1;
        [SerializeField] private Terrain _ground;

        private ObjectPool<Good> _goodPool;
        private float LastSpawnTime;
        private int _countSpawnGoods = 0;

        private void Awake()
        {
            _goodPool = new ObjectPool<Good>(
                createFunc: () => CreatePooledGood(),
                actionOnGet: (good) => ActionOnGet(good),
                actionOnRelease: (good) => ActionOnRelease(good),
                actionOnDestroy: (good) => DestroyGood(good),
                collectionCheck: false);
        }

        private void Start()
        {
            InvokeRepeating(nameof(GetGood), 0f, _repeatRate);
        }

        private Good CreatePooledGood()
        {
            Good good = Instantiate(_goodPrefab);
            good.gameObject.SetActive(false);
            good.Collected += ReturnGoodToPool;

            return good;
        }

        private void ActionOnGet(Good good)
        {
            good.transform.position = SelectSpawnPoint();
            good.gameObject.SetActive(true);
        }

        private void ActionOnRelease(Good good)
        {
            good.gameObject.SetActive(false);
        }

        private void DestroyGood(Good good)
        {
            good.Collected -= ReturnGoodToPool;
            Destroy(good);
        }

        private void GetGood()
        {
            if (_maxCountGoods > _countSpawnGoods)
            {
                _goodPool.Get();
                _countSpawnGoods++;
            }
        }

        private void ReturnGoodToPool(Good good)
        {
            _countSpawnGoods--;
            _goodPool.Release(good);
        }

        private Vector3 SelectSpawnPoint()
        {
            bool isSelectPoint = true;
            float deadZoneDistance = _goodPrefab.transform.localScale.x;
            Vector3 spawnPoint = Vector3.one;

            while (isSelectPoint)
            {
                spawnPoint = new Vector3(
                    Random.Range(0, _ground.terrainData.size.x),
                    spawnPoint.y,
                    Random.Range(0, _ground.terrainData.size.z));

                if (Physics.Raycast(spawnPoint, transform.forward, deadZoneDistance) == false)
                    isSelectPoint = false;
            }

            return spawnPoint;
        }
    }
}
