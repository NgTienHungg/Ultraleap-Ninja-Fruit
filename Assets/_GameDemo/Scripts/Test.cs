using Leap;
using UnityEngine;

public class Test : MonoBehaviour
{
    public LeapProvider leapProvider;

    public void OnEnable()
    {
        leapProvider.OnUpdateFrame += OnUpdateFrame;
    }

    private void OnDisable()
    {
        leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    private void OnUpdateFrame(Frame frame)
    {
        Debug.Log("On update frame");

        //Get a list of all the hands in the frame and loop through
        //to find the first hand that matches the Chirality
        foreach (var hand in frame.Hands)
        {
            if (hand.IsRight)
            {
                // Debug.Log("This is left hand: " + hand);
                var palmPosition = hand.PalmPosition;
                Debug.Log("Hand ID: " + hand.Id + " Palm Position: " + palmPosition);
            }
        }
    }
}