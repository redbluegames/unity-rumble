namespace RedBlueGames.Tools
{
    using System.Collections.Generic;

    /// <summary>
    /// Extensions to the List collection
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Removes all null entries in the list.
        /// </summary>
        /// <param name="list">List to modify.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void RemoveNullEntries<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                list.RemoveAll(RemoveNullEntriesHelper<T>.IsNull);
            }
        }

        /// <summary>
        /// Used to store a static delegate to prevent allocations in the above method.
        /// </summary>
        private static class RemoveNullEntriesHelper<T>
        {
            public static readonly System.Predicate<T> IsNull =
                (item => item == null || item.Equals(null));
        }
    }
}