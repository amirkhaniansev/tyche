using System;
using System.Collections.Generic;

namespace TycheBL
{
    /// <summary>
    /// Extensions for enumerable
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Enumerates in collection invoking the given action for every element.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="enumerable">enumerable</param>
        /// <param name="action">action</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            var enumerator = enumerable.GetEnumerator();
            if (enumerator == null)
                return;

            while (enumerator.MoveNext())
            {
                action.Invoke(enumerator.Current);
            }
        }
    }
}