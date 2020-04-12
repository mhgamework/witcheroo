using System;
using UnityEngine;

namespace Assets.TheWizards.Mathematics
{
    public static class RayExtensions
    {
        public static Ray Transform(this Ray ray, Matrix4x4 transform)
        {
            return new Ray(transform.MultiplyPoint(ray.origin), transform.MultiplyVector(ray.direction));
        }

        public static bool Intersects(this Ray ray, ref BoundingSphere sphere, out float result)
        {
            result = float.MaxValue;
            // Find the vector between where the ray starts the the sphere's centre
            Vector3 difference = sphere.position - ray.origin;


            float differenceLengthSquared = difference.sqrMagnitude;
            float sphereRadiusSquared = sphere.radius * sphere.radius;


            float distanceAlongRay;


            // If the distance between the ray start and the sphere's centre is less than
            // the radius of the sphere, it means we've intersected. N.B. checking the LengthSquared is faster.
            if (differenceLengthSquared < sphereRadiusSquared)
            {
                result = 0.0f;
                return true;
            }


            distanceAlongRay = Vector3.Dot(ray.direction, difference);
            // If the ray is pointing away from the sphere then we don't ever intersect
            if (distanceAlongRay < 0)
            {
                return false;
            }


            // Next we kinda use Pythagoras to check if we are within the bounds of the sphere
            // if x = radius of sphere
            // if y = distance between ray position and sphere centre
            // if z = the distance we've travelled along the ray
            // if x^2 + z^2 - y^2 < 0, we do not intersect
            var dist = sphereRadiusSquared + distanceAlongRay * distanceAlongRay - differenceLengthSquared;

            if (dist < 0) return false;
            result = distanceAlongRay - Mathf.Sqrt(dist);
            return true;
        }
        
        public static bool Intersects(this Ray2D ray, ref Vector2 sphereCenter, float radius, out float result)
        {
            result = float.MaxValue;
            // Find the vector between where the ray starts the the sphere's centre
            Vector2 difference = sphereCenter - ray.origin;


            float differenceLengthSquared = difference.sqrMagnitude;
            float sphereRadiusSquared = radius * radius;


            float distanceAlongRay;


            // If the distance between the ray start and the sphere's centre is less than
            // the radius of the sphere, it means we've intersected. N.B. checking the LengthSquared is faster.
            if (differenceLengthSquared < sphereRadiusSquared)
            {
                result = 0.0f;
                return true;
            }


            distanceAlongRay = Vector2.Dot(ray.direction, difference);
            // If the ray is pointing away from the sphere then we don't ever intersect
            if (distanceAlongRay < 0)
            {
                return false;
            }


            // Next we kinda use Pythagoras to check if we are within the bounds of the sphere
            // if x = radius of sphere
            // if y = distance between ray position and sphere centre
            // if z = the distance we've travelled along the ray
            // if x^2 + z^2 - y^2 < 0, we do not intersect
            var dist = sphereRadiusSquared + distanceAlongRay * distanceAlongRay - differenceLengthSquared;

            if (dist < 0) return false;
            result = distanceAlongRay - Mathf.Sqrt(dist);
            return true;
        }

    }
}