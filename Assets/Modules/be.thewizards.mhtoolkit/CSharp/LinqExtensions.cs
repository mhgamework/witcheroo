using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static int Max<T>(this IEnumerable<T> list,Func<T,int> selector, int defaultValue)
        {
            if (!list.Any()) return defaultValue;
            return list.Max(selector);
        }
        public static float Max<T>(this IEnumerable<T> list, Func<T, float> selector, float defaultValue)
        {
            if (!list.Any()) return defaultValue;
            return list.Max(selector);
        }

        [CanBeNull]
        public static T Smallest<T>(this List<T> list, List<float> value, T defaultValue = default(T))
        {
            T smallest = defaultValue;
            float smallestOrd = default;
            bool first = true;

            var listCount = list.Count;
            for (var i = 0; i < listCount; i++)
            {
                var f = list[i];
                var ord = value[i];
                if (first)
                {
                    smallestOrd = ord;
                    smallest = f;
                    first = false;
                    continue;
                }

                if (ord <  smallestOrd)
                {
                    smallest = f;
                    smallestOrd = ord;
                }
            }

            return smallest;
        }
        
        [CanBeNull]
        public static T Smallest<T,TOrd>(this IEnumerable<T> list, Func<T, TOrd> selector, T defaultValue = default(T))
        {
            T smallest = defaultValue;
            TOrd smallestOrd = default;
            bool first = true;

            var comp = Comparer<TOrd>.Default;
            foreach (var f in list)
            {
                var ord = selector(f);
                if (first)
                {
                    smallestOrd = ord;
                    smallest = f;
                    first = false;
                    continue;
                }

                if (comp.Compare(ord,smallestOrd) < 0)
                {
                    smallest = f;
                    smallestOrd = ord;
                }
            }

            return smallest;
        }
        public static T Smallest<T,TOrd>(this List<T> list, Func<T, TOrd> selector, T defaultValue = default(T))
        {
            T smallest = defaultValue;
            TOrd smallestOrd = default;
            bool first = true;

            var comp = Comparer<TOrd>.Default;
            var listCount = list.Count;
            for (var i = 0; i < listCount; i++)
            {
                var f = list[i];
                var ord = selector(f);
                if (first)
                {
                    smallestOrd = ord;
                    smallest = f;
                    first = false;
                    continue;
    }

                if (comp.Compare(ord, smallestOrd) < 0)
                {
                    smallest = f;
                    smallestOrd = ord;
                }
            }

            return smallest;
        }
        public static T SmallestFloat<T>(this List<T> list, Func<T, float> selector, T defaultValue = default(T))
        {
            T smallest = defaultValue;
            float smallestOrd = default;
            bool first = true;

            var listCount = list.Count;
            for (var i = 0; i < listCount; i++)
            {
                var f = list[i];
                var ord = selector(f);
                if (first)
                {
                    smallestOrd = ord;
                    smallest = f;
                    first = false;
                    continue;
                }

                if (ord < smallestOrd)
                {
                    smallest = f;
                    smallestOrd = ord;
                }
            }

            return smallest;
        }

        public static string Join(this IEnumerable<string> parts, string separator)
        {
            return string.Join(separator, parts);
        }
    }
}