using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public Fruit fruitPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 1.5f;
    public float shootForce = 5f;

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
        var fruit = Instantiate(fruitPrefab, spawnPoint.position, Quaternion.identity);
        fruit.AddForce(Vector2.up * shootForce);
    }
}