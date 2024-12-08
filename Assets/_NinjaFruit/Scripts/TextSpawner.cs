using BaseSource;
using UnityEngine;

namespace NinjaFruit
{
    public class TextSpawner : MonoSingleton<TextSpawner>
    {
        public GameObject[] listFxTextPrefab;
        public float offsetZ = -1;
        public Vector2 angleRange = new Vector2(-60f, 60f); // goc hop le
        public Vector2 clampAngleRange = new Vector2(-30f, 30f); // goc se clamp ve
        public float lifeTime = 5f;

        protected override void OnAwake() { }

        public void SpawnTextPerfect(Vector3 position, float angle)
        {
            var fxClone = Instantiate(listFxTextPrefab.Rand());

            fxClone.transform.position = position + new Vector3(0, 0, offsetZ);
            angle = angle.InRange(angleRange.x, angleRange.y)
                ? Mathf.Clamp(angle, clampAngleRange.x, clampAngleRange.y)
                : 0f; // khi chem doc qua thi coi nhu goc ngang
            fxClone.transform.localEulerAngles = new Vector3(0, 0, angle);

            // auto destroy
            Destroy(fxClone, lifeTime);
        }
    }
}