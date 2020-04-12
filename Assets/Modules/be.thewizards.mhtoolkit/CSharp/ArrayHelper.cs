namespace Assets.Modules.FormationPathfinding._Move
{
    public static class ArrayHelper
    {
        /// <summary>
        /// Does not keep original contents!
        /// </summary>
        public static T[] EnsureLength<T>(this T[] array, int length)
        {
            if (array == null || array.Length < length)
                return new T[length];
            return array;
        }
    }
}