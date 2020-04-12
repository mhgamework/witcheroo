using UnityEngine;

namespace Assets.MHGameWork.Reusable.Raycasting
{
    public static class RaycastingExtensions
    {
        public static Ray GetCenterScreenRay(this Camera cam)
        {
            return cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        }

        public static Ray GetCursorRay(this Camera cam)
        {
            return cam.ScreenPointToRay(Input.mousePosition);
        }
        public static Ray GetMousePositionRay(this Camera cam,Vector3 mousePosition)
        {
            return cam.ScreenPointToRay(mousePosition);
        }
    }
}