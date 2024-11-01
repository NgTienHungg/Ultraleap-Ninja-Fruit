using UnityEngine;

namespace NinjaFruit
{
    public class Fruit : MonoBehaviour
    {
        public bool autoDestroy;
        public float lifeTime = 5f;
        public float maxTorque = 0.1f;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            if (autoDestroy)
            {
                Destroy(gameObject, lifeTime);
            }
        }

        public void AddForce(Vector2 force, ForceMode forceMode = ForceMode.Impulse)
        {
            rb.AddForce(force, forceMode);

            // Thêm lực xoay ngẫu nhiên để fruit tự xoay
            Vector3 randomTorque = new Vector3(
                Random.Range(-maxTorque, maxTorque), // Xoay ngẫu nhiên quanh trục X
                Random.Range(-maxTorque, maxTorque), // Xoay ngẫu nhiên quanh trục Y
                Random.Range(-maxTorque, maxTorque) // Xoay ngẫu nhiên quanh trục Z
            );
            rb.AddTorque(randomTorque);
        }
    }
}