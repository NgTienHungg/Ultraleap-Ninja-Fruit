using BaseSource;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NinjaFruit
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public GameObject uiHome;
        public GameObject uiMainGame;
        public Image fadeImage;

        protected override void OnAwake() { }

        public void OpenHome()
        {
            uiHome.SetActive(true);
            uiMainGame.SetActive(false);
        }

        public void ShowUIMainGame()
        {
            uiHome.SetActive(false);
            uiMainGame.SetActive(true);
        }

        public void OnButtonPlay()
        {
            GameController.Instance.NewGame();
        }

        public UniTask FadeIn()
        {
            return fadeImage.DOColor(Color.white, 0.5f)
                .SetUpdate(true).ToUniTask();
        }

        public UniTask FadeOut()
        {
            return fadeImage.DOColor(Color.clear, 0.5f)
                .SetUpdate(true).ToUniTask();
        }
    }
}