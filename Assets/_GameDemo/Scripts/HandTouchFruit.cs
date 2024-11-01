using UnityEngine;

public class HandTouchFruit : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Fruit"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}