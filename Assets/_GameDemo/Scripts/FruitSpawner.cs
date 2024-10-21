using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public Fruit fruitPrefab;
    public Transform spawnPoint;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var fruit = Instantiate(fruitPrefab);
            fruit.transform.position = spawnPoint.position;
        }
    }
}