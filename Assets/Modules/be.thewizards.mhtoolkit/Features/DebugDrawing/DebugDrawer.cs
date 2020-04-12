using Assets.Modules.Utilities.Splines;
using MHGameWork.TheWizards;
using MHGameWork.TheWizards.Graphics;
using UnityEngine;

namespace Assets.Modules.Utilities.DebugDrawing
{
    public class DebugDrawer
    {
        private LineManager3D lineManager = new LineManager3D();
        public static DebugDrawer Get { get; } = new DebugDrawer();

        private int lineCount = 0;
        bool gizmos = false;
        Matrix4x4 matrix = Matrix4x4.identity;
        public float Duration = 0;

        void SetColor(Color color)
        {
            if (gizmos && UnityEngine.Gizmos.color != color) UnityEngine.Gizmos.color = color;
        }

        public void Line(Vector3 a, Vector3 b, Color color)
        {
            SetColor(color);
            lineCount++;
            if (gizmos) UnityEngine.Gizmos.DrawLine(matrix.MultiplyPoint3x4(a), matrix.MultiplyPoint3x4(b));
            else UnityEngine.Debug.DrawLine(matrix.MultiplyPoint3x4(a), matrix.MultiplyPoint3x4(b), color, Duration);
        }

        public void LineAngle(Vector3 a, float angle, float length, Color color)
        {
            DebugDrawer.Get.Line(
                a,
                a + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)) *
                length, color);
        }

        public void SquareXZ(Vector2 min, Vector2 max, Color color, float y = 0)
        {
            var diff = max - min;
            var diffx = new Vector2(diff.x, 0);
            var diffy = new Vector2(0, diff.y);
            Line(min.ToXZ(y), (min + diffx).ToXZ(y), color);
            Line(min.ToXZ(y), (min + diffy).ToXZ(y), color);
            Line(max.ToXZ(y), (min + diffx).ToXZ(y), color);
            Line(max.ToXZ(y), (min + diffy).ToXZ(y), color);
        }

        public void Triangle(Vector3 v1, Vector3 v2, Vector3 v3, Color color)
        {
            lineManager.AddTriangle(v1, v2, v3, color);
        }

        public void Rectangle(Vector3 center, Vector2 size, Vector3 dir1, Vector3 planeDir, Color color)
        {
            lineManager.AddRectangle(center, size, dir1, planeDir, color);
        }

        public void Box(Bounds box, Color col)
        {
            lineManager.AddBox(box, col);
        }

        public void CenteredBox(Vector3 center, float size, Color col)
        {
            lineManager.AddCenteredBox(center, size, col);
        }

        public void ViewFrustum(Vector3[] corners, Color color)
        {
            lineManager.AddViewFrustum(corners, color);
        }

        public void CircleXZ(Vector3 center, float radius, Color color, float startAngle = 0,
                             float endAngle = 6.283185f)
        {
            int steps = 40;

#if UNITY_EDITOR
            if (gizmos)
                steps = (int) Mathf.Clamp(
                    Mathf.Sqrt(radius / UnityEditor.HandleUtility.GetHandleSize(
                                   (UnityEngine.Gizmos.matrix * matrix)
                                  .MultiplyPoint3x4(center))) * 25, 4, 40);
#endif
            while (startAngle > endAngle) startAngle -= 2 * Mathf.PI;

            Vector3 prev = new Vector3(Mathf.Cos(startAngle) * radius, 0, Mathf.Sin(startAngle) * radius);
            for (int i = 0; i <= steps; i++)
            {
                Vector3 c = new Vector3(Mathf.Cos(Mathf.Lerp(startAngle, endAngle, i / (float) steps)) * radius, 0,
                                        Mathf.Sin(Mathf.Lerp(startAngle, endAngle, i / (float) steps)) * radius);
                Line(center + prev, center + c, color);
                prev = c;
            }
        }

        public void Cylinder(Vector3 position, Vector3 direction, float length, float radius, Color color)
        {
            var tangent = Vector3.Cross(direction, Vector3.one).normalized;

            matrix = Matrix4x4.TRS(position, Quaternion.LookRotation(tangent, direction),
                                   new Vector3(radius, length, radius));
            CircleXZ(Vector3.zero, 1, color);

            if (length > 0)
            {
                CircleXZ(Vector3.up, 1, color);
                Line(new Vector3(1, 0, 0), new Vector3(1, 1, 0), color);
                Line(new Vector3(-1, 0, 0), new Vector3(-1, 1, 0), color);
                Line(new Vector3(0, 0, 1), new Vector3(0, 1, 1), color);
                Line(new Vector3(0, 0, -1), new Vector3(0, 1, -1), color);
            }

            matrix = Matrix4x4.identity;
        }

        public void CrossXZ(Vector3 position, Color color, float size = 1)
        {
            size *= 0.5f;
            Line(position - Vector3.right * size, position + Vector3.right * size, color);
            Line(position - Vector3.forward * size, position + Vector3.forward * size, color);
        }

        public void Bezier(Vector3 a, Vector3 b, Color color)
        {
            Vector3 dir = b - a;

            if (dir == Vector3.zero) return;

            Vector3 normal = Vector3.Cross(Vector3.up, dir);
            Vector3 normalUp = Vector3.Cross(dir, normal);

            normalUp = normalUp.normalized;
            normalUp *= dir.magnitude * 0.1f;

            Vector3 p1c = a + normalUp;
            Vector3 p2c = b + normalUp;

            Vector3 prev = a;
            for (int i = 1; i <= 20; i++)
            {
                float t = i / 20.0f;
                Vector3 p = AstarSplines.CubicBezier(a, p1c, p2c, b, t);
                Line(prev, p, color);
                prev = p;
            }
        }


        public void Ray(Ray ray, float mag, Color color)
        {
            Line(ray.origin, ray.direction * mag + ray.origin, color);
        }

        public int GetAndResetLineCount()
        {
            var ret = lineCount;
            lineCount = 0;
            return ret;
        }
    }
}