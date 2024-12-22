using System.Collections;
using UnityEngine;

namespace NinjaFruit
{
    [RequireComponent(typeof(Collider))]
    public class FruitSpawner : MonoBehaviour
    {
        public Collider spawnArea;

        [Space]
        public GameObject[] fruitPrefabs;

        [Space]
        public GameObject bombPrefab;
        [Range(0f, 1f)]
        public float bombChance = 0.05f;

        [Space]
        public float minSpawnDelay = 0.25f;
        public float maxSpawnDelay = 1f;
        public float minAngle = -15f;
        public float maxAngle = 15f;
        public float minForce = 18f;
        public float maxForce = 22f;
        public float maxLifetime = 5f;
        // private void OnEnable()
        // {
        //     StartCoroutine(Spawn());
        // }
        //
        // private void OnDisable()
        // {
        //     StopAllCoroutines();
        // }

        public IEnumerator Spawn()
        {
            yield return new WaitForSeconds(1f);

            while (enabled)
            {
                // select prefab fruit / bomb
                GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
                if (Random.value < bombChance) prefab = bombPrefab;

                // random position, rotation, and instantiate
                Vector3 position = new Vector3
                {
                    x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                    y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                    z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
                };
                Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
                GameObject fruit = Instantiate(prefab, position, rotation);
                Destroy(fruit, maxLifetime);

                // Add upward force
                float force = Random.Range(minForce, maxForce);
                fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);

                yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            }
        }
    }
}