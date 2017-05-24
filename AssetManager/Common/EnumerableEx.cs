using System.Collections.Generic;
using System.Linq;

namespace AssetManager.Common
{
    public static class EnumerableEx
    {
        public static IEnumerable<T> Concat<T>(params IEnumerable<T>[] items)
        {
            IEnumerable<T> result = items.First();

            foreach (var item in items.Skip(1))
                result = result.Concat(item);

            return result;
        }
    }
}
