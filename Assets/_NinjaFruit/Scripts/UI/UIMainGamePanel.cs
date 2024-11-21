using BaseSource;
using TMPro;

namespace NinjaFruit
{
    public class UIMainGamePanel : UIPanel
    {
        public TextMeshProUGUI scoreTxt;

        public void Setup()
        {
            scoreTxt.text = "0";
        }

        private void FixedUpdate()
        {
            scoreTxt.text = GameManager.Instance.score.ToString();
        }
    }
}