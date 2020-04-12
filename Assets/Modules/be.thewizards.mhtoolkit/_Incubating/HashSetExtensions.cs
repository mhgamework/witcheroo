using System.Collections.Generic;

namespace Modules.MoveScenario._Test._Incubating
{
    public static class HashSetExtensions
    {
        public static bool TryAdd<T>(this HashSet<T> set,T val)
        {
            if (set.Contains(val)) return false;
            set.Add(val);
            return true;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> e)
        {
            return new HashSet<T>(e);
        }
    }
}