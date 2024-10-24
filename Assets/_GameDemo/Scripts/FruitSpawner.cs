using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public Fruit fruitPrefab;
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
        var spawnX = Random.Range(bottomLeftPoint.position.x, bottomRightPoint.position.x);
        var spawnPos = new Vector3(spawnX, bottomLeftPoint.position.y, 0f);
        var fruit = Instantiate(fruitPrefab, spawnPos, Quaternion.identity);

        var shootForce = Random.Range(minShootForce, maxShootForce);
        var directionX = Random.Range(-horizontalForce, horizontalForce);
        var force = new Vector2(directionX, shootForce);
        fruit.AddForce(force);
    }
}