using System;
using System.Collections.Generic;

namespace GraphQL.Data.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> If<T>(this IEnumerable<T> enumerable, bool condition,
            Func<IEnumerable<T>, IEnumerable<T>> action)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return condition ? action(enumerable) : enumerable;
        }
    }
}