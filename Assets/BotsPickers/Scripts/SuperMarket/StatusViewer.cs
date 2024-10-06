using TMPro;
using UnityEngine;

namespace BotsPickers
{
    public class StatusViewer : MonoBehaviour
    {
        [SerializeField] private Counter _counter;
        [SerializeField] private TMP_Text _showGoodsCount;
        [SerializeField] private TMP_Text _showTruckCount;

        private void OnEnable()
        {
            _counter.ScoreRecalculated += ShowGoodsCount;
            _counter.TruckRecalculated += ShowTruckCount;
        }

        private void OnDisable()
        {
            _counter.ScoreRecalculated -= ShowGoodsCount;
            _counter.TruckRecalculated -= ShowTruckCount;
        }

        private void ShowTruckCount(int truckCount)
        {
            _showTruckCount.text = truckCount.ToString();
        }

        private void ShowGoodsCount(int goodsCount)
        {
            _showGoodsCount.text = goodsCount.ToString();
        }
    }
}