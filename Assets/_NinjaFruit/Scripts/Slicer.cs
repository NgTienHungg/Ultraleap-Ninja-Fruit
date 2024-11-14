using Hanzzz.MeshSlicerFree;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NinjaFruit
{
    public class Slicer : MonoBehaviour
    {
        public GameObject fruitModel;
        public Material intersectionMaterial;
        public float splitDistance = 0.1f;

        private MeshSlicer meshSlicer = new MeshSlicer();
        private (GameObject, GameObject) result;

        public Transform slicePlane;
        private float sliceForce;

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

        private void PreSliceOperation()
        {
            if (result.Item1 != null)
            {
                DestroyImmediate(result.Item1);
                DestroyImmediate(result.Item2);
                result = (null, null);
            }
        }

        private void PostSliceOperation()
        {
            if (result.Item1 == null)
            {
                Debug.LogError("Slice plane does not intersect slide target");
                return;
            }

            result.Item1.transform.SetParent(transform, false);
            result.Item2.transform.SetParent(transform, false);
            result.Item1.transform.position += splitDistance * slicePlane.up;
            result.Item2.transform.position -= splitDistance * slicePlane.up;
            fruitModel.SetActive(false);

            // Add a force to each slice based on the blade direction
            var rb1 = result.Item1.AddComponent<Rigidbody>();
            rb1.velocity = fruitModel.GetComponent<Rigidbody>().velocity;
            rb1.AddForce(sliceForce * slicePlane.up, ForceMode.Impulse);

            var rb2 = result.Item2.AddComponent<Rigidbody>();
            rb2.velocity = fruitModel.GetComponent<Rigidbody>().velocity;
            rb2.AddForce(-sliceForce * slicePlane.up, ForceMode.Impulse);
        }

        public void Slice(Transform slicer, float force)
        {
            slicePlane = slicer;
            sliceForce = force;

            PreSliceOperation();
            result = meshSlicer.Slice(fruitModel, Get3PointsOnPlane(new Plane(slicePlane.up, slicePlane.position)), intersectionMaterial);
            PostSliceOperation();
        }

        [Button]
        public void SliceTest()
        {
            Slice(slicePlane, 0f);
        }

        [Button]
        [ContextMenu("Clear")]
        public void Clear()
        {
            if (null != result.Item1)
            {
                DestroyImmediate(result.Item1);
                DestroyImmediate(result.Item2);
                result = (null, null);
            }

            meshSlicer = new MeshSlicer();
            fruitModel.SetActive(true);
        }
    }
}