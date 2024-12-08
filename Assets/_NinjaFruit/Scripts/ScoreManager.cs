using BaseSource;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace NinjaFruit
{
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        public TextMeshProUGUI txtScore;

        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                txtScore.text = Score.ToString();
            }
        }

        public static int BestScore
        {
            get => PlayerPrefs.GetInt("BestScore", 0);
            set => PlayerPrefs.SetInt("BestScore", value);
        }

        protected override void OnAwake() { }

        public void ResetScore()
        {
            Score = 0;
        }

        public void IncreaseScore(int pts)
        {
            Score += pts;

            txtScore.transform.DOKill();
            txtScore.transform.localScale = Vector3.one * 1.5f;
            txtScore.transform.DOScale(Vector3.one * 1f, 0.2f)
                .SetEase(Ease.OutQuad).SetUpdate(true);
        }

        public void UpdateBestScore()
        {
            BestScore = Mathf.Max(BestScore, Score);
        }
    }
}