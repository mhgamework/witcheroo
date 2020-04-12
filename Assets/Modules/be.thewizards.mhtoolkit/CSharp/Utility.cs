namespace Modules.MHGameWork.Reusable.Utils
{
    public static class Utility
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            var f = a;
            a = b;
            b = f;
        }
    }
}