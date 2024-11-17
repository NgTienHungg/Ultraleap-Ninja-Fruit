using Hanzzz.MeshSlicerFree;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NinjaFruit
{
    public class FruitSlicer : MonoBehaviour
    {
        public GameObject fruitModel;
        public Material intersectionMaterial;
        public float splitDistance = 0.1f;
        public ParticleSystem juiceEffect;

        private Rigidbody fruitRigidbody;
        private Collider fruitCollider;

        private MeshSlicer meshSlicer = new MeshSlicer();
        private (GameObject, GameObject) result;
        private Blade blade;

        private void Awake()
        {
            fruitRigidbody = GetComponent<Rigidbody>();
            fruitCollider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Blade blade = other.GetComponent<Blade>();
                this.blade = blade;
                Slice();
            }
        }

        [Button]
        public void Slice()
        {
            PreSliceOperation();
            result = meshSlicer.Slice(fruitModel, Get3PointsOnPlane(new Plane(blade.transform.up, blade.transform.position)), intersectionMaterial);
            PostSliceOperation();
        }

        private (Vector3, Vector3, Vector3) Get3PointsOnPlane(Plane p)
        {
            Vector3 xAxis;
            if (0f != p.normal.x)
            {
                xAxis = new Vector3(-p.normal.y / p.normal.x, 1f, 0f);
            }
            else if (0f != p.normal.y)
            {
                xAxis = new Vector3(0f, -p.normal.z / p.normal.y, 1f);
            }
            else
            {
                xAxis = new Vector3(1f, 0f, -p.normal.x / p.normal.z);
            }

            Vector3 yAxis = Vector3.Cross(p.normal, xAxis);
            return (-p.distance * p.normal, -p.distance * p.normal + xAxis, -p.distance * p.normal + yAxis);
        }

        private void PreSliceOperation()
        {
            // if (result.Item1 != null)
            // {
            //     DestroyImmediate(result.Item1);
            //     DestroyImmediate(result.Item2);
            //     result = (null, null);
            // }
        }

        private void PostSliceOperation()
        {
            if (result.Item1 == null)
            {
                Debug.LogError("Slice plane does not intersect slide target");
                return;
            }

            fruitCollider.enabled = false;
            fruitModel.SetActive(false);
            juiceEffect.Play();

            float angle = Mathf.Atan2(blade.direction.y, blade.direction.x) * Mathf.Rad2Deg;
            result.Item1.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            result.Item2.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            result.Item1.transform.SetParent(transform, false);
            result.Item2.transform.SetParent(transform, false);
            result.Item1.transform.position += splitDistance * blade.transform.up;
            result.Item2.transform.position -= splitDistance * blade.transform.up;

            // Add a force to each slice based on the blade direction
            var rb1 = result.Item1.AddComponent<Rigidbody>();
            rb1.velocity = fruitRigidbody.velocity;
            rb1.AddForce(blade.sliceForce * blade.transform.up, ForceMode.Impulse);

            var rb2 = result.Item2.AddComponent<Rigidbody>();
            rb2.velocity = fruitRigidbody.velocity;
            rb2.AddForce(-blade.sliceForce * blade.transform.up, ForceMode.Impulse);
        }

        [Button]
        public void Clear()
        {
            if (null != result.Item1)
            {
                DestroyImmediate(result.Item1);
                DestroyImmediate(result.Item2);
                result = (null, null);
            }

            meshSlicer = new MeshSlicer();
            fruitModel.SetActive(true);
        }
    }
}