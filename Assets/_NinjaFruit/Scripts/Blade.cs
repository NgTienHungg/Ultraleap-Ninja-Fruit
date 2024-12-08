using Leap;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils = BaseSource.Utils;

namespace NinjaFruit
{
    public class Blade : MonoBehaviour
    {
        public bool useHand;
        [ShowIf("@useHand")] public Chirality handType;
        [ShowIf("@useHand")] public Finger.FingerType fingerType = Finger.FingerType.INDEX;

        [Space]
        public Collider sliceCollider;
        public float sliceForce = 5f;

        [Space]
        public TrailRenderer sliceTrail;
        public float minSliceVelocity = 0.01f;

        private Camera mainCamera;
        private LeapProvider leapProvider;
        private Hand hand;

        public Vector3 direction { get; private set; }
        public bool slicing { get; private set; }

        private void Awake()
        {
            mainCamera = Camera.main;
            leapProvider = Hands.Provider;
        }

        private void OnEnable()
        {
            StopSlice();
            leapProvider.OnUpdateFrame += OnUpdateFrame;
        }

        private void OnDisable()
        {
            StopSlice();
            leapProvider.OnUpdateFrame -= OnUpdateFrame;
        }

        private void OnUpdateFrame(Frame frame)
        {
            if (!useHand)
                return;

            hand = frame.GetHand(handType);
            if (hand != null)
            {
                if (!slicing)
                    StartSlice();
                else
                    ContinueSlice();
            }
            else
            {
                if (slicing)
                    StopSlice();
            }
        }

        #region === MOUSE ===
        private void Update()
        {
            if (useHand)
                return;

            if (Input.GetMouseButtonDown(0) && Utils.IsMouseOverUI())
                return;

            if (Input.GetMouseButtonDown(0))
                StartSlice();
            else if (Input.GetMouseButtonUp(0))
                StopSlice();
            else if (slicing)
                ContinueSlice();
        }

        private void StartSlice()
        {
            Vector3 position;
            if (useHand)
                position = hand.fingers[(int)fingerType].TipPosition;
            else
                position = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            position.z = 0f;
            transform.position = position;

            slicing = true;
            sliceCollider.enabled = true;
            sliceTrail.enabled = true;
            sliceTrail.Clear();
        }

        private void ContinueSlice()
        {
            Vector3 newPosition;
            if (useHand)
                newPosition = hand.fingers[(int)fingerType].TipPosition;
            else
                newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            newPosition.z = 0f;
            direction = newPosition - transform.position;

            float velocity = direction.magnitude / Time.deltaTime;
            bool canSlice = velocity >= minSliceVelocity;
            sliceCollider.enabled = canSlice;
            transform.localPosition = newPosition;
        }

        private void StopSlice()
        {
            slicing = false;
            sliceCollider.enabled = false;
            sliceTrail.enabled = false;
        }
        #endregion
    }
}