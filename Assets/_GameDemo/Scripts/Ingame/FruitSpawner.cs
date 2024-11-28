using UnityEngine;

namespace GameDemo
{
    public class FruitSpawner : MonoBehaviour
    {
        public Fruit[] fruitPrefabs;
        public Transform bottomLeftPoint;
        public Transform bottomRightPoint;
        public float spawnInterval = 1.5f;
        public float minShootForce = 5f;
        public float maxShootForce = 10f;
        public float horizontalForce = 2f;

        private float timer;

        private void Start()
        {
            timer = 0f;
        }

        public void Update()
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                SpawnAndShootFruit();
                timer = 0f;
            }
        }

        private void SpawnAndShootFruit()
        {
            var randomPosX = Random.Range(bottomLeftPoint.position.x, bottomRightPoint.position.x);
            var spawnPos = new Vector3(randomPosX, bottomLeftPoint.position.y, bottomLeftPoint.position.z);
            var randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            var fruit = Instantiate(fruitPrefabs.Rand(), spawnPos, randomRotation);

            var shootForce = Random.Range(minShootForce, maxShootForce);
            var directionX = randomPosX < 0f ? Random.Range(0, horizontalForce) : Random.Range(-horizontalForce, 0);
            var force = new Vector2(directionX, shootForce);
            fruit.AddForce(force);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(bottomLeftPoint.position, bottomRightPoint.position);
        }
    }
}