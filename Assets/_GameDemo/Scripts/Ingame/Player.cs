using Leap;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils = BaseSource.Utils;
using Vector3 = UnityEngine.Vector3;

namespace NinjaFruit
{
    public class Player : MonoBehaviour
    {
        [Title("Hand")]
        public bool useMouse;
        public bool priorRightHand;
        public Finger.FingerType fingerType = Finger.FingerType.INDEX;

        [Title("Trail")]
        public GameObject trail;
        public float minDistanceActiveTrail = 0.1f;

        private LeapProvider _leapProvider;
        private Vector3 _lastPos;

        private void Awake()
        {
            _leapProvider = Hands.Provider;
        }

        private void OnEnable()
        {
            _leapProvider.OnUpdateFrame += OnUpdateFrame;
        }

        private void OnDisable()
        {
            _leapProvider.OnUpdateFrame -= OnUpdateFrame;
        }

        private void OnUpdateFrame(Frame frame)
        {
            if (useMouse) return;
            
            // neu uu tien tay phai
            if (priorRightHand)
            {
                var rightHand = frame.GetHand(Chirality.Right);
                if (rightHand != null)
                {
                    var fingerPos = rightHand.fingers[(int)fingerType];
                    UpdateTrail(fingerPos.TipPosition);
                    return;
                }
            }

            var leftHand = frame.GetHand(Chirality.Left);
            if (leftHand != null)
            {
                var fingerPos = leftHand.fingers[(int)fingerType];
                UpdateTrail(fingerPos.TipPosition);
                return;
            }

            //
            // if (leftHand == null && rightHand == null)
            // {
            //     trail.SetActive(false);
            //     return;
            // }
            //
            // var hand = frame.GetHand(useRightHand ? Chirality.Right : Chirality.Left);
            //
            // if (hand == null)
            // {
            //     trail.SetActive(false);
            //     return;
            // }
            //
            // var finger = hand.fingers[(int)fingerType];
            // if (Vector3.Distance(finger.TipPosition, _lastPos) >= minDistanceActiveTrail)
            // {
            //     trail.SetActive(true);
            //     trail.transform.position = finger.TipPosition;
            // }
            // else
            // {
            //     trail.SetActive(false);
            // }
            //
            // _lastPos = finger.TipPosition;
        }

        private void Update()
        {
            if (useMouse)
            {
                UpdateTrail(Utils.GetMouseWorldPosition());
            }
        }

        private void UpdateTrail(Vector3 position)
        {
            if (Vector3.Distance(position, _lastPos) >= minDistanceActiveTrail)
            {
                trail.SetActive(true);
                trail.transform.position = position;
            }
            else
            {
                trail.SetActive(false);
            }

            _lastPos = position;
        }
    }
}