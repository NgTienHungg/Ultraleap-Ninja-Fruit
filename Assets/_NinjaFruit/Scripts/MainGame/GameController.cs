using BaseSource;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace NinjaFruit
{
    [DefaultExecutionOrder(-1)]
    public class GameController : MonoSingleton<GameController>
    {
        public UIMainGamePanel uiMainGamePanel;
        [SerializeField] private Blade blade;
        [SerializeField] private Spawner spawner;
        [SerializeField] private Image fadeImage;

        protected override void OnAwake() { }

        private void Start()
        {
            NewGame();
        }

        private void NewGame()
        {
            Time.timeScale = 1f;

            ClearScene();

            blade.enabled = true;
            spawner.enabled = true;

            ScoreManager.Instance.ResetScore();
            uiMainGamePanel.Setup();
        }

        private void ClearScene()
        {
            Fruit[] fruits = FindObjectsOfType<Fruit>();
            foreach (var fruit in fruits)
            {
                Destroy(fruit.gameObject);
            }

            Bomb[] bombs = FindObjectsOfType<Bomb>();
            foreach (Bomb bomb in bombs)
            {
                Destroy(bomb.gameObject);
            }
        }

        public void Explode()
        {
            blade.enabled = false;
            spawner.enabled = false;
            ExplodeSequence();
        }

        private async void ExplodeSequence()
        {
            await DOVirtual.Float(1f, 0.2f, 1f, (t) => Time.timeScale = t)
                .SetEase(Ease.InCubic).SetUpdate(true).ToUniTask();

            await fadeImage.DOColor(Color.white, 0.5f).SetUpdate(true).ToUniTask();
            await UniTask.Delay(500, ignoreTimeScale: true);

            NewGame();

            await fadeImage.DOColor(Color.clear, 0.5f).SetUpdate(true).ToUniTask();
        }
    }
}