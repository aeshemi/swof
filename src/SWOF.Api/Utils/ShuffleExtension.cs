using System;
using System.Collections.Generic;
using System.Linq;

namespace SWOF.Api.Utils
{
	public static class ShuffleExtension
	{
		private static readonly Random random = new Random();

		// Knuth-Fisher-Yates Shuffle
		public static List<int> Shuffle(this IEnumerable<int> sequence)
		{
			var list = sequence.ToList();
			var lastItem = list.Last();

			for (var n = list.Count - 1; n > 0; --n)
			{
				var swapIndex = random.Next(n + 1);
				list.Swap(swapIndex, n);
			}

			// Add first item in shuffle not same as last item in original sequence check
			if (list.First() == lastItem)
				list.Swap(random.Next(1, list.Count));

			return list;
		}

		private static void Swap<T>(this IList<T> list, int swapIndex, int index = 0)
		{
			var temp = list[index];
			list[index] = list[swapIndex];
			list[swapIndex] = temp;
		}
	}
}
