using Hanzzz.MeshSlicerFree;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NinjaFruit
{
    public class Fruit : MonoBehaviour
    {
        [Header("Fruit Settings")]
        public int points = 1;
        public GameObject fruitModel;
        public Material intersectionMaterial;
        public float splitDistance = 0.1f;

        [Header("Effects")]
        public ParticleSystem juiceEffect;

        private Rigidbody fruitRigidbody;
        private Collider fruitCollider;
        private MeshSlicer meshSlicer = new MeshSlicer();
        private (GameObject, GameObject) slicedResults = (null, null);
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
                blade = other.GetComponent<Blade>();
                Slice();
            }
        }

        [Button]
        public void Slice()
        {
            ClearPreviousSlices();
            if (TrySlice())
            {
                HandlePostSliceEffects();
            }
        }

        private void ClearPreviousSlices()
        {
            if (slicedResults.Item1 != null)
            {
                DestroyImmediate(slicedResults.Item1);
                DestroyImmediate(slicedResults.Item2);
                slicedResults = (null, null);
            }
        }

        private bool TrySlice()
        {
            Plane slicePlane = new Plane(blade.transform.up, blade.transform.position);
            var pointsOnPlane = GetPointsOnPlane(slicePlane);

            slicedResults = meshSlicer.Slice(fruitModel, pointsOnPlane, intersectionMaterial);

            if (slicedResults.Item1 == null)
            {
                Debug.LogError("Slice plane does not intersect the target.");
                return false;
            }

            return true;
        }

        private void HandlePostSliceEffects()
        {
            DisableFruitModel();
            PlayJuiceEffect();
            AlignSlices();
            AddPhysicsToSlices();
            ScoreManager.Instance.IncreaseScore(points);
        }

        private void DisableFruitModel()
        {
            fruitCollider.enabled = false;
            fruitModel.SetActive(false);
        }

        private void PlayJuiceEffect()
        {
            juiceEffect.Play();
        }

        private void AlignSlices()
        {
            float angle = Mathf.Atan2(blade.direction.y, blade.direction.x) * Mathf.Rad2Deg;
            AlignSlice(slicedResults.Item1, angle, splitDistance);
            AlignSlice(slicedResults.Item2, angle, -splitDistance);
        }

        private void AlignSlice(GameObject slice, float angle, float offset)
        {
            slice.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            slice.transform.SetParent(transform, false);
            slice.transform.position += offset * blade.transform.up;
        }

        private void AddPhysicsToSlices()
        {
            AddPhysicsToSlice(slicedResults.Item1, blade.sliceForce);
            AddPhysicsToSlice(slicedResults.Item2, -blade.sliceForce);
        }

        private void AddPhysicsToSlice(GameObject slice, float forceDirection)
        {
            var rb = slice.AddComponent<Rigidbody>();
            rb.velocity = fruitRigidbody.velocity;
            rb.AddForceAtPosition(forceDirection * blade.transform.up, blade.transform.position, ForceMode.Impulse);
        }

        private (Vector3, Vector3, Vector3) GetPointsOnPlane(Plane plane)
        {
            Vector3 xAxis = GetNonParallelAxis(plane.normal);
            Vector3 yAxis = Vector3.Cross(plane.normal, xAxis);
            Vector3 planeOrigin = -plane.distance * plane.normal;
            return (planeOrigin, planeOrigin + xAxis, planeOrigin + yAxis);
        }

        private Vector3 GetNonParallelAxis(Vector3 normal)
        {
            if (normal.x != 0f)
                return new Vector3(-normal.y / normal.x, 1f, 0f);
            if (normal.y != 0f)
                return new Vector3(0f, -normal.z / normal.y, 1f);
            return new Vector3(1f, 0f, -normal.x / normal.z);
        }

        [Button]
        public void Clear()
        {
            ClearPreviousSlices();
            meshSlicer = new MeshSlicer();
            fruitModel.SetActive(true);
        }
    }
}