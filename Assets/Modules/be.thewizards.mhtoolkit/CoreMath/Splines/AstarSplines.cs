using UnityEngine;

namespace Assets.Modules.Utilities.Splines
{
    /// <summary>
    /// COPIED FROM ASTAR PATHFINDING
    /// Contains various spline functions.
    /// \ingroup utils
    /// </summary>
    static class AstarSplines
    {
        public static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime)
        {
            // References used:
            // p.266 GemsV1
            //
            // tension is often set to 0.5 but you can use any reasonable value:
            // http://www.cs.cmu.edu/~462/projects/assn2/assn2/catmullRom.pdf
            //
            // bias and tension controls:
            // http://local.wasp.uwa.edu.au/~pbourke/miscellaneous/interpolation/

            float percentComplete = elapsedTime;
            float percentCompleteSquared = percentComplete * percentComplete;
            float percentCompleteCubed = percentCompleteSquared * percentComplete;

            return
                previous * (-0.5F * percentCompleteCubed +
                            percentCompleteSquared -
                            0.5F * percentComplete) +

                start *
                (1.5F * percentCompleteCubed +
                 -2.5F * percentCompleteSquared + 1.0F) +

                end *
                (-1.5F * percentCompleteCubed +
                 2.0F * percentCompleteSquared +
                 0.5F * percentComplete) +

                next *
                (0.5F * percentCompleteCubed -
                 0.5F * percentCompleteSquared);
        }

        /// <summary>Returns a point on a cubic bezier curve. t is clamped between 0 and 1</summary>
        public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float t2 = 1 - t;
            return t2 * t2 * t2 * p0 + 3 * t2 * t2 * t * p1 + 3 * t2 * t * t * p2 + t * t * t * p3;
        }

        /// <summary>Returns the derivative for a point on a cubic bezier curve. t is clamped between 0 and 1</summary>
        public static Vector3 CubicBezierDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float t2 = 1 - t;
            return 3 * t2 * t2 * (p1 - p0) + 6 * t2 * t * (p2 - p1) + 3 * t * t * (p3 - p2);
        }

        /// <summary>Returns the second derivative for a point on a cubic bezier curve. t is clamped between 0 and 1</summary>
        public static Vector3 CubicBezierSecondDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float t2 = 1 - t;
            return 6 * t2 * (p2 - 2 * p1 + p0) + 6 * t * (p3 - 2 * p2 + p1);
        }
    }
}