using System;
using System.Collections.Generic;
using System.Linq;

namespace SWOF.Api.Utils
{
public static class ShuffleExtension
{
private static readonly Random random = new Random();

/****************************************************************************************

Implementation of Knuth-Fisher-Yates Shuffle.


INPUT:

IN_ciSequence           Current date to start iteration of scheduling shifts


RESULT:

Tuple with the scheduled rotation of engineers and the last scheduled date for the rotation.

****************************************************************************************/
//QQQ
                public static List<int>
Shuffle
               (this IEnumerable<int>       IN_ciSequence)  // long type and would not fit in column 37
{
var             list                = IN_ciSequence.ToList();

var             lastItem            = list.Last();

/*

Following loop iterates a collection of integers and shuffles its order.

*/

for (var n = list.Count - 1; n > 0; --n)
    {
    var swapIndex = random.Next(n + 1);

    list.Swap(swapIndex, n);
    }

// Add first item in shuffle not same as last item in original sequence check

if (list.First() == lastItem) list.Swap(random.Next(1, list.Count));

return          list;
}


/****************************************************************************************

Extension function to swap the index of items in a collection.


INPUT:

IN_lList                Collection to swap indexes of items
IN_iSwapIndex           Index where to swap the item for the given index to
IN_iIndex               Index of the item to be swapped

****************************************************************************************/
//QQQ

                private static void
Swap<T>
               (this IList<T>       list,
                int                 swapIndex,
                int                 index = 0)
{
var             temp                = list[index];

list[index] = list[swapIndex];

list[swapIndex] = temp;
}
}
}
