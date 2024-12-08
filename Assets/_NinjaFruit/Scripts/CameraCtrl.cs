using DG.Tweening;
using UnityEngine;

namespace NinjaFruit
{
    public class CameraCtrl : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float duration = 0.5f; // Thời gian rung
        [SerializeField] private float strength = 0.2f; // Cường độ rung
        [SerializeField] private int vibrato = 10; // Số lần rung trong thời gian
        [SerializeField] private float randomness = 90; // Độ ngẫu nhiên của hướng rung

        public void ShakeCamera()
        {
            if (mainCamera != null)
            {
                mainCamera.transform.DOShakePosition(duration, strength, vibrato, randomness);
            }
        }
    }
}