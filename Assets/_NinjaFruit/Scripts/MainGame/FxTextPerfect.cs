using UnityEngine;

namespace NinjaFruit
{
    public class FxTextPerfect : MonoBehaviour
    {
        public float offsetZ = 1f;
        public Vector2 angleRange = new Vector2(-60f, 60f); // goc hop le
        public Vector2 clampAngleRange = new Vector2(-30f, 30f); // goc se clamp ve

        public void Setup(Vector3 position, float angle)
        {
            transform.position = position + new Vector3(0, 0, offsetZ);

            angle = angle.InRange(angleRange.x, angleRange.y)
                ? Mathf.Clamp(angle, clampAngleRange.x, clampAngleRange.y)
                : 0f; // khi chem doc qua thi coi nhu goc ngang

            transform.localEulerAngles = new Vector3(0, 0, angle);
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}