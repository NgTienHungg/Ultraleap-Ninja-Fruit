using TMPro;
using UnityEngine;

namespace NinjaFruit
{
    public class UIHome : MonoBehaviour
    {
        public TextMeshProUGUI txtBestScore;

        public void OnEnable()
        {
            txtBestScore.text = ScoreManager.BestScore.ToString();
        }

        public void OnButtonPlay()
        {
            GameController.Instance.NewGame();
        }
    }
}