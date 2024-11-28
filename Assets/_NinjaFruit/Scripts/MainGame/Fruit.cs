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
            var pointsOnPlane = Get3PointsOnPlane(slicePlane);

            slicedResults = meshSlicer.Slice(fruitModel, pointsOnPlane, intersectionMaterial);

            if (slicedResults.Item1 == null)
            {
                Debug.LogError("Slice plane does not intersect the target.");
                return false;
            }

            slicedResults.Item1.gameObject.name = "Slice 1";
            slicedResults.Item2.gameObject.name = "Slice 2";

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
            // Sử dụng mặt phẳng cắt để xác định hướng tách
            Plane slicePlane = new Plane(blade.transform.up, blade.transform.position);

            AlignSlice(slicedResults.Item1, blade.direction, splitDistance);
            AlignSlice(slicedResults.Item2, blade.direction, -splitDistance);
        }


        private void AlignSlice(GameObject slice, Vector3 normal, float offset)
        {
            // Tính toán góc xoay từ pháp tuyến của mặt phẳng
            Quaternion sliceRotation = Quaternion.LookRotation(Vector3.forward, normal);

            // Đặt góc xoay của lát cắt
            slice.transform.rotation = sliceRotation;

            // Tách lát ra bằng cách dịch chuyển theo pháp tuyến của mặt phẳng
            slice.transform.SetParent(transform, false);
            slice.transform.position += normal * offset;
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

        [Button]
        public void Clear()
        {
            ClearPreviousSlices();
            meshSlicer = new MeshSlicer();
            fruitModel.SetActive(true);
        }
    }
}