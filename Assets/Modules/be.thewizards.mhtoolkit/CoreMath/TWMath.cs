using System;
using DirectX11;
using UnityEngine;

namespace MHGameWork.TheWizards
{
    public class TWMath
    {
        public static float Min(float a, float b, float c)
        {
            var ret = a;
            if (b < ret) ret = b;
            if (c < ret) ret = c;
            return ret;
        }
        public static float Max(float a, float b, float c)
        {
            var ret = a;
            if (b > ret) ret = b;
            if (c > ret) ret = c;
            return ret;
        }
        public static float Min(float a, float b, float c,float d)
        {
            var ret = a;
            if (b < ret) ret = b;
            if (c < ret) ret = c;
            if (d < ret) ret = d;
            return ret;
        }
        public static float Max(float a, float b, float c, float d)
        {
            var ret = a;
            if (b > ret) ret = b;
            if (c > ret) ret = c;
            if (d > ret) ret = d;
            return ret;
        }


        /// <summary>
        /// Looparound modulo, works with negative numbers
        /// </summary>
        public static int nfmod(int a, int b)
        {
            // Test shows this is fastest
            var r = a % b;
            return r < 0 ? r + b : r;
            //return (a - b * (int)Math.Floor(a / (double)b));
        }
        /// <summary>
        /// Looparound modulo, works with negative numbers
        /// </summary>  
        public static float nfmod(float a, float b)
        {
            // Test shows this is fastest
            var r = a % b;
            return r < 0 ? r + b : r;
            //return (float)(a - b * Math.Floor(a / b));
        }



        /// <summary>
        /// Performs trilinear interpolation on the q-values (qXYZ) which are positioned at the corners of a cube from (0,0,0) to (1,1,1)
        /// The factor is the sample point inside this unit cube.
        /// </summary>
        public static float triLerp(Vector3 factor, float q000, float q100, float q001, float q101, float q010, float q110,
                                     float q011, float q111)
        {
            var x00 = Mathf.Lerp(q000, q100, factor.x);
            var x01 = Mathf.Lerp(q001, q101, factor.x);
            var x10 = Mathf.Lerp(q010, q110, factor.x);
            var x11 = Mathf.Lerp(q011, q111, factor.x);

            var y0 = Mathf.Lerp(x00, x10, factor.y);
            var y1 = Mathf.Lerp(x01, x11, factor.y);


            var z = Mathf.Lerp(y0, y1, factor.z);
            return z;
        }
        
        public static Vector3 Bilerp(Vector3 x0y0, Vector3 x1y0, Vector3 x0y1, Vector3 x1y1, Vector2 t)
        {
            Vector3 a = Vector3.LerpUnclamped(x0y0, x1y0, t.x);
            Vector3 b = Vector3.LerpUnclamped(x0y1, x1y1, t.x);
            return Vector3.LerpUnclamped(a, b, t.y);
        }
        public static float Bilerp(float x0y0, float x1y0, float x0y1, float x1y1, Vector2 t)
        {
            var a = Mathf.LerpUnclamped(x0y0, x1y0, t.x);
            var b = Mathf.LerpUnclamped(x0y1, x1y1, t.x);
            return Mathf.LerpUnclamped(a, b, t.y);
        }


        public static Vector3 UnitX { get {return new Vector3(1,0,0);} }
        public static Vector3 UnitY { get {return new Vector3(0,1,0);} }
        public static Vector3 UnitZ { get {return new Vector3(0,0,1);} }
    }
}