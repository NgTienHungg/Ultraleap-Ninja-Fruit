using Leap;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class TrailFollowFinger : MonoBehaviour
{
    [SerializeField] private Finger.FingerType fingerType;
    [SerializeField] private GameObject trail;
    [SerializeField] private float minDistanceActiveTrail = 1f;

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
        var hand = frame.GetHand(Chirality.Right);

        if (hand == null)
        {
            trail.SetActive(false);
            return;
        }

        var finger = hand.fingers[(int)fingerType];
        if (Vector3.Distance(finger.TipPosition, _lastPos) >= minDistanceActiveTrail)
        {
            trail.SetActive(true);
            trail.transform.position = finger.TipPosition;
        }
        else
        {
            trail.SetActive(false);
        }

        _lastPos = finger.TipPosition;
    }
}