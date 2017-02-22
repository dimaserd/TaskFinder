using System.Collections.Generic;
using System.Linq;

namespace Extensions.Int
{
    public static class IntExtensions
    {
        public static int GetMinOf(params int[] ints)
        {
            return ints.Min();
        }

        public static int GetMinOf(IEnumerable<int> ints)
        {
            return ints.Min();
        }
    }
}
