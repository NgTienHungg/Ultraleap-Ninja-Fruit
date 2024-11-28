using UnityEngine;

namespace NinjaFruit
{
    public class Blade : MonoBehaviour
    {
        public Collider sliceCollider;
        public float sliceForce = 5f;

        [Space]
        public TrailRenderer sliceTrail;
        public float minSliceVelocity = 0.01f;

        private Camera mainCamera;

        public Vector3 direction { get; private set; }
        public bool slicing { get; private set; }

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            StopSlice();
        }

        private void OnDisable()
        {
            StopSlice();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartSlice();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StopSlice();
            }
            else if (slicing)
            {
                ContinueSlice();
            }
        }

        private void StartSlice()
        {
            Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0f;
            transform.position = position;

            slicing = true;
            sliceCollider.enabled = true;

            sliceTrail.gameObject.SetActive(true);
            sliceTrail.enabled = true;
            sliceTrail.Clear();
        }

        private void StopSlice()
        {
            slicing = false;
            sliceCollider.enabled = false;

            sliceTrail.gameObject.SetActive(false);
            sliceTrail.enabled = false;
        }

        private void ContinueSlice()
        {
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0f;
            direction = newPosition - transform.position;

            float velocity = direction.magnitude / Time.deltaTime;
            bool canSlice = velocity >= minSliceVelocity;
            sliceCollider.enabled = canSlice;
            sliceTrail.gameObject.SetActive(canSlice);

            transform.localPosition = newPosition;
        }
    }
}