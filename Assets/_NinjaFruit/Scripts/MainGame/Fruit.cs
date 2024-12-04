using Hanzzz.MeshSlicerFree;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NinjaFruit
{
    public class Fruit : MonoBehaviour
    {
        [Header("Slice")]
        public float standardRadius = 1f;
        public float splitDistance = 0.1f;
        public GameObject fruitModel;
        public Material intersectionMaterial;
        public ParticleSystem juiceEffect;

        [Header("Point")]
        public int points = 1;
        public int perfectFactor = 2;
        public Vector2 sliceRange = new Vector2(0.35f, 0.65f);
        public Vector2 perfectRange = new Vector2(0.45f, 0.55f);

        private Rigidbody fruitRigidbody;
        private Collider fruitCollider;
        private MeshSlicer meshSlicer = new MeshSlicer();
        private (GameObject, GameObject) slicedResults = (null, null);
        private Blade blade;

        public float radius = 1f;
        private float sliceDistance;

        private void Awake()
        {
            fruitRigidbody = GetComponent<Rigidbody>();
            fruitCollider = GetComponent<Collider>();
            radius = standardRadius * transform.localScale.x;
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

        #region ========== TEST ==========
        [Space]
        public Transform testBlade;

        [Button]
        public void TestSlice()
        {
            ClearPreviousSlices();

            Plane slicePlane = new Plane(testBlade.up, testBlade.position);
            float distanceFromPlaneToFruit = slicePlane.GetDistanceToPoint(transform.position);
            Debug.Log($"Khoảng cách ban đầu từ tâm đến mặt phẳng cắt: {distanceFromPlaneToFruit}");

            // Giới hạn khoảng cách trong sliceRange
            // Mathf.Sign(distanceFromPlaneToFruit) *
            //     Mathf.Clamp(Mathf.Abs(distanceFromPlaneToFruit),

            float clampedDistance = Mathf.Clamp(distanceFromPlaneToFruit, sliceRange.x * radius, sliceRange.y * radius);

            // Nếu khoảng cách đã bị clamp, cập nhật vị trí mặt phẳng
            if (Mathf.Abs(clampedDistance - distanceFromPlaneToFruit) > Mathf.Epsilon)
            {
                Debug.Log($"Clamp khoảng cách từ {distanceFromPlaneToFruit} thành {clampedDistance}");

                // Dịch mặt phẳng cắt lại gần tâm
                Vector3 offset = (clampedDistance - distanceFromPlaneToFruit) * slicePlane.normal;
                slicePlane.Translate(offset);
                // slicePlane = new Plane(slicePlane.normal, slicePlane.distance + (clampedDistance - distance));
                // testBlade.position += offset; // Cập nhật vị trí testBlade
            }

            Debug.Log("Radius: " + transform.localScale.x * standardRadius);

            //new Plane(testBlade.transform.up, testBlade.transform.position)
            slicedResults = meshSlicer.Slice(fruitModel, Get3PointsOnPlane(slicePlane), intersectionMaterial);

            if (null == slicedResults.Item1)
            {
                Debug.Log("Slice plane does not intersect slice target.");
                return;
            }

            slicedResults.Item1.transform.SetParent(transform, false);
            slicedResults.Item2.transform.SetParent(transform, false);
            slicedResults.Item1.transform.position += splitDistance * testBlade.up;
            slicedResults.Item2.transform.position -= splitDistance * testBlade.up;
            fruitModel.SetActive(false);
        }
        #endregion

        private bool TrySlice()
        {
            Plane slicePlane = new Plane(blade.transform.up, blade.transform.position);
            float distanceFromPlaneToFruit = slicePlane.GetDistanceToPoint(transform.position); // Tính khoảng cách từ tâm đến mặt phẳng cắt
            sliceDistance = Mathf.Clamp(distanceFromPlaneToFruit, sliceRange.x * radius, sliceRange.y * radius); // Giới hạn khoảng cách trong sliceRange

            // Nếu khoảng cách đã bị clamp, cập nhật vị trí mặt phẳng
            if (Mathf.Abs(sliceDistance - distanceFromPlaneToFruit) > Mathf.Epsilon)
            {
                Debug.Log($"Clamp khoảng cách từ {distanceFromPlaneToFruit} thành {sliceDistance}");
                Vector3 offset = (sliceDistance - distanceFromPlaneToFruit) * slicePlane.normal;
                slicePlane.Translate(offset); // Dịch mặt phẳng cắt lại gần tâm
            }

            slicedResults = meshSlicer.Slice(fruitModel, Get3PointsOnPlane(slicePlane), intersectionMaterial);
            if (slicedResults.Item1 == null)
            {
                Debug.LogError("Slice plane does not intersect the target.");
                return false;
            }

            return true;
        }

        private void HandlePostSliceEffects()
        {
            // old fruit
            fruitCollider.enabled = false;
            fruitModel.SetActive(false);
            juiceEffect.Play();

            AlignSlices();
            AddPhysicsToSlices();
            HandleScore();
        }

        private void AlignSlices()
        {
            // Sử dụng mặt phẳng cắt để xác định hướng tách
            float angle = Mathf.Atan2(blade.direction.y, blade.direction.x) * Mathf.Rad2Deg;
            Debug.Log($"Direction: {blade.direction}, Angle: {angle}".Color("yellow"));

            if (Mathf.Abs(angle) > 90)
            {
                // giữ cho Item 1 luôn ở trên, Item 2 ở dưới
                slicedResults = (slicedResults.Item2, slicedResults.Item1);
            }

            slicedResults.Item1.gameObject.name = "Slice 1";
            slicedResults.Item2.gameObject.name = "Slice 2";

            AlignSlice(slicedResults.Item1, blade.direction, splitDistance, angle);
            AlignSlice(slicedResults.Item2, blade.direction, -splitDistance, angle);
        }

        private void AlignSlice(GameObject slice, Vector3 direction, float offset, float angle)
        {
            // Tách lát ra bằng cách dịch chuyển theo pháp tuyến của mặt phẳng
            slice.transform.SetParent(transform, false);
            slice.transform.position += direction * offset;
            slice.transform.rotation = Quaternion.Euler(0f, 0f, angle);
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

        private void HandleScore()
        {
            var score = points;
            if (sliceDistance.InRange(radius * perfectRange.x, radius * perfectRange.y))
            {
                score *= perfectFactor;
                var fxText = FxSpawner.Instance.SpawnFxTextPerfect();
                fxText.Setup(transform.position, slicedResults.Item1.transform.eulerAngles.z);
            }

            ScoreManager.Instance.IncreaseScore(score);
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