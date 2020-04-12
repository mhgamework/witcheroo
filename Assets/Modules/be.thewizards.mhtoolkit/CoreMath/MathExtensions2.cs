using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.UnityAdditions
{
    public static class MathExtensions
    {
        /// <summary>
        /// Component wise multiplication
        /// </summary>
        /// <returns></returns>
        public static Vector3 Multiply(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        /// <summary>
        /// Component wise multiplication
        /// </summary>
        /// <returns></returns>
        public static Vector2 Multiply(this Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        /// <summary>
        /// Component wise Abs
        /// </summary>
        /// <returns></returns>
        public static Vector3 Abs(this Vector3 a)
        {
            return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
        }

        /// <summary>
        /// Component wise Abs
        /// </summary>
        /// <returns></returns>
        public static Vector2 Abs(this Vector2 a)
        {
            return new Vector2(Mathf.Abs(a.x), Mathf.Abs(a.y));
        }

        /// <summary>
        /// Inclusive min max check
        /// </summary>
        public static bool IsInRange(this float v, float min, float max)
        {
            return v >= min && v <= max;
        }

        /// <summary>
        /// Inclusive min max check
        /// </summary>
        public static bool IsInRange(this Vector2 v, float min, float max)
        {
            return v.x >= min && v.x <= max && v.y >= min && v.y <= max;
        }

        /// <summary>
        /// Inclusive min max check
        /// </summary>
        public static bool IsInRange(this Vector2 v, Vector2 min, Vector2 max)
        {
            return v.x >= min.x && v.x <= max.x && v.y >= min.y && v.y <= max.y;
        }

        /// <summary>
        /// Inclusive min max check
        /// </summary>
        public static bool IsInRange(this Vector3 v, float min, float max)
        {
            return v.x >= min && v.x <= max && v.y >= min && v.y <= max && v.z >= min && v.z <= max;
        }

        /// <summary>
        /// Inclusive min max check
        /// </summary>
        public static bool IsInRange(this Vector3 v, Vector3 min, Vector3 max)
        {
            return v.x >= min.x && v.x <= max.x && v.y >= min.y && v.y <= max.y && v.z >= min.z && v.z <= max.z;
        }
    }
}