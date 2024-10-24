using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Space]
    [SerializeField] private float maxForce = 10f;
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void AddForce(Vector2 force, ForceMode forceMode = ForceMode.Impulse)
    {
        rb.AddForce(force, forceMode);
    }

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");

        // Tạo vector lực ngẫu nhiên
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized; // Chuẩn hóa để lực có độ dài không đổi

        // Tạo lực ngẫu nhiên có giá trị trong khoảng từ 0 đến maxForce
        float randomForce = Random.Range(0f, maxForce);

        // Áp dụng lực vào quả cầu theo hướng ngẫu nhiên
        rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);
    }
}