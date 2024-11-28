using BaseSource;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace NinjaFruit
{
    public class UIMainGamePanel : UIPanel
    {
        public TextMeshProUGUI scoreTxt;

        public void Setup()
        {
            scoreTxt.text = "0";
        }

        private void OnEnable()
        {
            this.RegisterListener(Constant.Event.OnScoreChanged, OnScoreChanged);
        }

        private void OnDisable()
        {
            this.RemoveListener(Constant.Event.OnScoreChanged, OnScoreChanged);
        }

        private void OnScoreChanged(object scoreChanged)
        {
            scoreTxt.text = ScoreManager.Instance.Score.ToString();

            scoreTxt.transform.DOKill();
            scoreTxt.transform.localScale = Vector3.one * 1.3f;
            scoreTxt.transform.DOScale(Vector3.one * 1f, 0.2f)
                .SetEase(Ease.OutQuad).SetUpdate(true);
        }

        // private void FixedUpdate()
        // {
        //     scoreTxt.text = GameManager.Instance.score.ToString();
        // }
    }
}