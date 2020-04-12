using MHGameWork.TheWizards;
using UnityEngine;

namespace Assets.Homm
{
    /// <summary>
    /// TODO: transform mouse delta to world coords so movement feels natural
    /// </summary>
    public class MapCameraInputScript : MonoBehaviour
    {

        public float ScrollSpeed = 1;
        /// <summary>
        /// For when user leaves screen
        /// </summary>
        public float MaxDeltaThreshold = 10;

        public float MinZoom = 0.5f;
        public float MaxZoom = 2;
        public float ZoomSpeed = 0.9f;
        private float zoom = 1;

        private float startY;
        private float startSize;
        public void Start()
        {
            lastMousePos = Input.mousePosition;
            startY = transform.position.y;
            cam = GetComponent<Camera>();
            startSize = cam.orthographicSize;
        }

        private Vector3 lastMousePos;
        private Camera cam;

        public void Update()
        {
            if (Input.GetMouseButton(2))
            {
                var delta = (Input.mousePosition - lastMousePos) * ScrollSpeed;
                if (delta.magnitude < MaxDeltaThreshold)
                {
                    transform.position += new Vector3(-delta.x, 0, -delta.y) * zoom;

                }
            }

            if (Input.mouseScrollDelta.y > 0) zoom *= ZoomSpeed;
            if (Input.mouseScrollDelta.y < 0) zoom /= ZoomSpeed;

            zoom = Mathf.Clamp(zoom, MinZoom, MaxZoom);
            if (cam.orthographic)
            {
                cam.orthographicSize = startSize * zoom;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, startY * zoom, transform.position.z);
            }

            lastMousePos = Input.mousePosition;
        }
    }
}