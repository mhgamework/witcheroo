using System.Collections.Generic;
using DirectX11;
using UnityEngine;

namespace Assets.Reusable
{
    public class Point2EqualityComparer : EqualityComparer<Point2>
    {
        public override int GetHashCode(Point2 obj)
        {
            return obj.GetHashCode();
        }

        public override bool Equals(Point2 x, Point2 y)
        {
            return x == y;
        }
    }
}