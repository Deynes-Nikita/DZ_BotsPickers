using UnityEngine;

namespace BotsPickers
{
    public class GoodsHandler : MonoBehaviour
    {
        private Vector3 _bootPoint;
        private Good _good;

        private void Start()
        {
            _bootPoint = GetComponentInChildren<BootPoint>().transform.localPosition;
        }

        public bool TryPickup(Good good, float interactionDistance)
        {
            if (good == null || good.IsBusy)
                return false;

            _good = good;

            _good.OnBook();

            _good.transform.SetParent(transform);
            _good.transform.localPosition = _bootPoint;
            _good.transform.localRotation = Quaternion.identity;

            return true;
        }

        public void Drop(Base targetBase)
        {
            if (_good == null)
                return;

            _good.transform.parent = null;
            targetBase.AcceptGood(_good);
        }
    }
}