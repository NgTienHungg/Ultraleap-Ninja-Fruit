using UnityEngine;

namespace NinjaFruit
{
    public class HandTouchFruit : MonoBehaviour
    {
        private Vector3 startPoint;
        private Vector3 endPoint; 
        private bool isDragging;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    startPoint = hit.point;
                }
            }

            if (Input.GetMouseButton(0) && isDragging)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    endPoint = hit.point;
                    DrawAndCheckCollision();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }

        private void DrawAndCheckCollision()
        {
            // Vẽ đường từ startPoint đến endPoint
            Debug.DrawLine(startPoint, endPoint, Color.red);

            // Kiểm tra va chạm với fruit dọc theo đường kẻ
            var direction = endPoint - startPoint;
            var distance = direction.magnitude;

            if (Physics.Raycast(startPoint, direction.normalized, out RaycastHit hit, distance))
            {
                if (hit.collider.CompareTag(GameConfig.Tag.FRUIT))
                {
                    Debug.Log("Hit fruit: " + hit.collider.name);
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (isDragging)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(startPoint, endPoint);
            }
        }
    }
}