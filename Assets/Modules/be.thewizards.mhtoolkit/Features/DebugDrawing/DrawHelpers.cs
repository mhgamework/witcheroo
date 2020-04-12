using System.Collections.Generic;
using Assets.CustomGridPathfindingTest;
using DirectX11;
using MHGameWork.TheWizards;
using UnityEngine;

namespace Assets.Utils
{
    public static class DrawHelpers
    {

        public static void DrawRect(Rect obstacle, Color color, float duration = 0, float y = 0)
        {
            var min = obstacle.min.ToXZ(y);
            var max = obstacle.max.ToXZ(y);
            Debug.DrawLine(min, (min + Vector3.right * obstacle.width), color, duration);
            Debug.DrawLine((min + Vector3.right * obstacle.width), max, color, duration);
            Debug.DrawLine(max, (min + Vector3.forward * obstacle.height), color, duration);
            Debug.DrawLine((min + Vector3.forward * obstacle.height), min, color, duration);
        }

        public static void DrawRect(OrientedRect rect, Color color, float duration = 0, float y = 0)
        {
            Debug.DrawLine(rect.LowerLeft.ToXZ(y), rect.LowerRight.ToXZ(y), color, duration);
            Debug.DrawLine(rect.LowerRight.ToXZ(y), rect.UpperRight.ToXZ(y), color, duration);
            Debug.DrawLine(rect.UpperRight.ToXZ(y), rect.UpperLeft.ToXZ(y), color, duration);
            Debug.DrawLine(rect.UpperLeft.ToXZ(y), rect.LowerLeft.ToXZ(y), color, duration);
        }


    }
}