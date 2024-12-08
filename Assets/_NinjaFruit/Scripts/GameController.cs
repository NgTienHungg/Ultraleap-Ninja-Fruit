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
        [SerializeField] private Blade blade;
        [SerializeField] private FruitSpawner fruitSpawner;
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
            fruitSpawner.enabled = true;

            ScoreManager.Instance.ResetScore();
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
            fruitSpawner.enabled = false;
            ExplodeSequence();
        }

        private async void ExplodeSequence()
        {
            await DOVirtual.Float(1f, 0.2f, 0.6f, (t) => Time.timeScale = t)
                .SetEase(Ease.InCubic).SetUpdate(true).ToUniTask();

            await fadeImage.DOColor(Color.white, 0.5f).SetUpdate(true).ToUniTask();
            await UniTask.Delay(500, ignoreTimeScale: true);

            NewGame();

            await fadeImage.DOColor(Color.clear, 0.5f).SetUpdate(true).ToUniTask();
        }
    }
}