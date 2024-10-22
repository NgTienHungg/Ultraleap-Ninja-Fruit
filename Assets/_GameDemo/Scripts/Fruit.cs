using UnityEngine;

public class Fruit : MonoBehaviour
{
    // Gắn Material của đối tượng vào đây thông qua Inspector
    public Renderer objectRenderer;

    [Space]
    public Rigidbody rb;
    public float maxForce = 10f;
    public float lifeTime = 5f;

    private void Start()
    {
        // Tạo màu ngẫu nhiên bằng cách sử dụng Random.ColorHSV()
        Color randomColor = Random.ColorHSV();

        // Gán màu ngẫu nhiên cho Material của đối tượng
        objectRenderer.material.color = randomColor;

        Destroy(gameObject, lifeTime);
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