using System;
using System.Collections.Generic;
using DirectX11;
using UnityEngine;

namespace MHGameWork.TheWizards
{
    /// <summary>
    /// Helper class for working with grids 2D and 3D
    /// </summary>
    public static class GridHelper
    {
        /// <summary>
        /// x,y,z
        /// </summary>
        public static readonly Point3[] Axes3D = new[] {new Point3(1, 0, 0), new Point3(0, 1, 0), new Point3(0, 0, 1)};

        /// <summary>
        /// x,y,z,-x,-y,-z
        /// </summary>
        public static readonly Point3[] OrthogonalDirections3D = new[]
        {
            new Point3(1, 0, 0), new Point3(0, 1, 0), new Point3(0, 0, 1),
            new Point3(-1, 0, 0), new Point3(0, -1, 0), new Point3(0, 0, -1)
        };

        public static readonly Point3[] UnitCubeCorners = new[]
        {
            new Point3(0, 0, 0), new Point3(1, 0, 0), new Point3(0, 1, 0), new Point3(0, 0, 1),
            new Point3(1, 1, 0), new Point3(0, 1, 1), new Point3(1, 0, 1), new Point3(1, 1, 1),
        };

        public static bool IsOutside(Point2 p, int size)
        {
            return p.X < 0 || p.Y < 0 ||
                   p.X >= size || p.Y >= size;
        }

        public static bool IsOutside(Point3 p, int size)
        {
            return p.X < 0 || p.Y < 0 || p.Z < 0 ||
                   p.X >= size || p.Y >= size || p.Z >= size;
        }

        public static Point2 ClampInside(Point2 p, int size)
        {
            return new Point2(
                Mathf.Clamp(p.X, 0, size - 1),
                Mathf.Clamp(p.Y, 0, size - 1));
        }

        public static Vector2 ClampPointInside(Vector2 p, float size)
        {
            return new Vector2(
                Mathf.Clamp(p.x, 0, size - 1),
                Mathf.Clamp(p.y, 0, size - 1));
        }

        public static void ForEach(int size, Action<Point2> action)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    action(new Point2(x, y));
                }
            }
        }
        public static void ForEachMinMaxInclusive(Point2 min, Point2 max, Action<Point2> action)
        {
            for (int y = min.Y; y <= max.Y; y++)
            {
                for (int x = min.X; x <= max.X; x++)
                {
                    action(new Point2(x, y));
                }
            }
        }

        public static void ForEach3(int size, Action<Point3> action)
        {
            for (int z = 0; z < size; z++)
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    action(new Point3(x, y, z));
                }
            }
        }
    }
}