using System.Collections.Generic;
using UnityEngine;

namespace BotsPickers
{
    public class Scanner : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;

        public void Scan(List<Good> goods)
        {
            foreach (Good good in GetGoods())
            {
                goods.Add(good);
            }
        }

        private List<Good> GetGoods()
        {
            List<Good> goods = new List<Good>();

            Collider[] hits = Physics.OverlapSphere(transform.position, _radius);

            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent<Good>(out Good good) && good.IsBusy == false)
                    goods.Add(good);
            }

            return goods;
        }
    }
}
