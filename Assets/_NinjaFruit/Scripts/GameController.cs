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
        public CameraCtrl cameraCtrl;
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
            ClearScene();

            blade.enabled = true;
            fruitSpawner.enabled = true;

            Time.timeScale = 1f;
            LifeManager.Instance.ResetLife();
            ScoreManager.Instance.ResetScore();
        }

        private void ClearScene()
        {
            foreach (var fruit in FindObjectsOfType<Fruit>())
                Destroy(fruit.gameObject);

            foreach (var bomb in FindObjectsOfType<Bomb>())
                Destroy(bomb.gameObject);
        }

        public void Explode()
        {
            LifeManager.Instance.Life--;
            cameraCtrl.ShakeCamera();

            if (LifeManager.Instance.IsOutOfLife)
            {
                blade.enabled = false;
                fruitSpawner.enabled = false;
                ExplodeSequence();
            }
        }

        private async void ExplodeSequence()
        {
            await DOVirtual.Float(1f, 0.2f, 1f, (t) => Time.timeScale = t)
                .SetEase(Ease.OutCubic).SetUpdate(true).ToUniTask();
            await fadeImage.DOColor(Color.white, 0.5f)
                .SetUpdate(true).ToUniTask();
            await UniTask.Delay(200, ignoreTimeScale: true);

            NewGame();

            await fadeImage.DOColor(Color.clear, 0.5f)
                .SetUpdate(true).ToUniTask();
        }
    }
}