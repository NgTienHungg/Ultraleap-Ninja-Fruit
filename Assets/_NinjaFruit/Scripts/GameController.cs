using BaseSource;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace NinjaFruit
{
    [DefaultExecutionOrder(-1)]
    public class GameController : MonoSingleton<GameController>
    {
        public CameraCtrl cameraCtrl;
        public Blade blade;
        public FruitSpawner fruitSpawner;

        protected override void OnAwake() { }

        private void Start()
        {
            UIManager.Instance.OpenHome();
        }

        public void NewGame()
        {
            Time.timeScale = 1f;
            UIManager.Instance.ShowUIMainGame();
            LifeManager.Instance.ResetLife();
            ScoreManager.Instance.ResetScore();

            ClearScene();
            blade.enabled = true;
            fruitSpawner.enabled = true;
            StartCoroutine(fruitSpawner.Spawn());
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
                ScoreManager.Instance.UpdateBestScore();

                ExplodeSequence();
            }
        }

        private async void ExplodeSequence()
        {
            await DOVirtual.Float(1f, 0.2f, 1f, (t) => Time.timeScale = t).SetEase(Ease.OutCubic).SetUpdate(true).ToUniTask();
            await UIManager.Instance.FadeIn();
            await UniTask.Delay(200, ignoreTimeScale: true);
            await UIManager.Instance.FadeOut();
            UIManager.Instance.OpenHome();
        }
    }
}