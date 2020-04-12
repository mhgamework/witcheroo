using System;
using UnityEngine;

namespace Modules.MHGameWork.Reusable.UnityAdditions.CustomPropertyDrawers
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class MinMaxPropertyAttribute : PropertyAttribute
    {
        public readonly float min;
        public readonly float max;

        public MinMaxPropertyAttribute() : this(0, 1) {}

        public MinMaxPropertyAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}