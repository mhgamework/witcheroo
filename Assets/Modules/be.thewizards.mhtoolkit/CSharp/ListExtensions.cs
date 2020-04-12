using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Modules.MHGameWork.Reusable.Utils
{
    public static class ListExtensions
    {
        /// <summary>
        /// Removes the given element by swapping it with the last element in the list, and removing the last element
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapRemoveAt<T>(this List<T> list, int index)
        {
	        if (list.Count == 1 && index == 0)
	        {
		        list.RemoveAt(0);
		        return;
	        } 
	        
	        var lastIndex = list.Count - 1;
	        list[index] = list[lastIndex];
	        list.RemoveAt(lastIndex);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SwapRemoveAt(this IList list, int index)
        {
	        if (list.Count == 1 && index == 0)
	        {
		        list.RemoveAt(0);
		        return;
	        }

	        var lastIndex = list.Count - 1;
	        list[index] = list[lastIndex];
	        list.RemoveAt(lastIndex);
        }
    }
}