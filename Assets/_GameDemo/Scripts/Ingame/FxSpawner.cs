using BaseSource;
using UnityEngine;

namespace GameDemo
{
    public class FxSpawner : MonoSingleton<FxSpawner>
    {
        public ParticleSystem fxFruitSlicedPrefab;

        protected override void OnAwake() { }

        public void SpawnFx(Vector3 position, float lifeTime = 5f)
        {
            var fx = Instantiate(fxFruitSlicedPrefab, position, Quaternion.identity);
            Destroy(fx.gameObject, lifeTime);
        }
    }
}