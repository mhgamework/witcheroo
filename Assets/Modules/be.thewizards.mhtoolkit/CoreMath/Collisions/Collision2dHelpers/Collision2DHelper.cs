using MHGameWork.TheWizards;
using UnityEngine;

namespace Assets.CustomGridPathfindingTest
{
    public static class Collision2DHelper
    {
        public static bool Intersects(this OrientedRect obb, Rect aabb)
        {
            return Intersects(aabb, obb);
        }

        public static bool Intersects(Rect aabb, OrientedRect obb)
        {
            var all = new Vector2(aabb.xMin, aabb.yMin);
            var alr = new Vector2(aabb.xMax, aabb.yMin);
            var aul = new Vector2(aabb.xMin, aabb.yMax);
            var aur = new Vector2(aabb.xMax, aabb.yMax);

            var bll = obb.LowerLeft;
            var blr = obb.LowerRight;
            var bul = obb.UpperLeft;
            var bur = obb.UpperRight;

            if (SeparatesOnAxis(Vector2.up, aabb.yMin, aabb.yMax, bll, blr, bul, bur)) return false;
            if (SeparatesOnAxis(Vector2.right, aabb.xMin, aabb.xMax, bll, blr, bul, bur)) return false;

            if (SeparatesOnAxis(obb.Forward, obb.ForwardMin, obb.ForwardMax, all, alr, aul, aur)) return false;
            if (SeparatesOnAxis(obb.Right, obb.RightMin, obb.RightMax, all, alr, aul, aur)) return false;

            return true;
        }

        private static bool SeparatesOnAxis(Vector2 axis, float aMin, float aMax, Vector2 b1, Vector2 b2, Vector2 b3, Vector2 b4)
        {
            var dotb1 = Vector2.Dot(axis, b1);
            var dotb2 = Vector2.Dot(axis, b2);
            var dotb3 = Vector2.Dot(axis, b3);
            var dotb4 = Vector2.Dot(axis, b4);

            var bMin = TWMath.Min(dotb1, dotb2, dotb3, dotb4);
            var bMax = TWMath.Max(dotb1, dotb2, dotb3, dotb4);


            if (aMin > bMax || bMin > aMax) return true;
            return false;
        }
    }

    public struct OrientedRect
    {
        private Vector2 rootPos;
        private Vector2 forward;
        private float length;
        private float width;

        public OrientedRect(Vector2 rootPos, Vector2 forward, float length, float width)
        {
            this.rootPos = rootPos;
            this.forward = forward;
            this.length = length;
            this.width = width;
        }

        public Vector2 LowerLeft => rootPos - Right * width / 2;
        public Vector2 LowerRight => rootPos + Right * width / 2;
        public Vector2 UpperLeft => rootPos - Right * width / 2 + forward * length;
        public Vector2 UpperRight => rootPos + Right * width / 2 + forward * length;
        public Vector2 Forward => forward;
        public Vector2 Right => new Vector2(forward.y, -forward.x);

        public float RightMin => Vector2.Dot(rootPos, Right) - width / 2;
        public float RightMax => RightMin + width;
        public float ForwardMin => Vector2.Dot(rootPos, Forward);
        public float ForwardMax => ForwardMin + length;

        public static OrientedRect FromStartEnd(Vector2 start, Vector2 end, float width)
        {
            var diff = end - start;
            return new OrientedRect(start, diff.normalized, diff.magnitude, width);
        }

        public Rect GetAABB()
        {
            var ll = LowerLeft;
            var lr = LowerRight;
            var ul = UpperLeft;
            var ur = UpperRight;
            var minx = TWMath.Min(ll.x, lr.x, ul.x, ur.x);
            var miny = TWMath.Min(ll.y, lr.y, ul.y, ur.y);
            var maxx = TWMath.Max(ll.x, lr.x, ul.x, ur.x);
            var maxy = TWMath.Max(ll.y, lr.y, ul.y, ur.y);

            return new Rect(minx, miny, maxx - minx, maxy - miny);
        }
    }
}