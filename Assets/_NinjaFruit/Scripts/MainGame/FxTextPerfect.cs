using UnityEngine;

namespace NinjaFruit
{
    public class FxTextPerfect : MonoBehaviour
    {
        // [SerializeField] private float angle = -15f;
        // [SerializeField] private float localZ = -5f;

        public void Setup(Vector3 position, float angle)
        {
            transform.position = position;
            transform.localEulerAngles = new Vector3(0, 0, angle);
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}