using System.Collections.ObjectModel;

namespace BookLibraryManager.Common.Util;

/// <summary>
/// Provides extension methods for working with collections.
/// </summary>
/// <author>YR 2025-02-06</author>
internal static class CollectionExtentions
{
    /// <summary>
    /// Performs the specified action on each element of the specified collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to iterate over.</param>
    /// <param name="action">The action to perform on each element.</param>
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        if (collection is null || !collection.Any()) return;

        foreach (var t in collection)
            action(t);
    }

    /// <summary>
    /// Removes all elements from the specified collection that match the specified condition.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to remove elements from.</param>
    /// <param name="condition">The condition to match for removal.</param>
    public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> condition)
    {
        if (collection?.Any() != true) return;

        collection.Where(condition).ToList().ForEach(e => collection.Remove(e));
    }

    /// <summary>
    /// Removes all occurrences of the specified items from the target collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collections.</typeparam>
    /// <param name="target">The collection to remove items from.</param>
    /// <param name="items">The list of items to remove.</param>
    public static void RemoveItems<T>(this ICollection<T> target, IList<T> items)
    {
        if (!target.Any() || !items.Any()) return;

        foreach (var item in items)
        {
            var found = target.FirstOrDefault(
                x => x != null && item != null && x.Equals(item));

            if (found != null)
                target.Remove(found);
        }
    }

    /// <summary>
    /// Removes the specified item from the target ObservableCollection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the ObservableCollection.</typeparam>
    /// <param name="target">The ObservableCollection to remove the item from.</param>
    /// <param name="item">The item to remove.</param>
    /// <returns>True if the item was removed, false otherwise.</returns>
    public static bool RemoveItem<T>(this ObservableCollection<T> target, T item)
    {
        if (target is null || item is null) 
            return false;

        var index = target.IndexOf(item);

        if (index >= 0 && index < target?.Count)
        {
            target.RemoveAt(index);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds a range of items to the specified ICollection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the ICollection.</typeparam>
    /// <param name="collection">The ICollection to add items to.</param>
    /// <param name="sourceNewItems">The IEnumerable of items to add.</param>
    public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> sourceNewItems)
    {
        if (collection == null || sourceNewItems?.Any() != true) return;

        sourceNewItems.ForEach(item =>
        {
            if (item != null)
                collection.Add(item);
        });
    }

    /// <summary>
    /// Resets the specified ICollection and adds a range of items.
    /// </summary>
    /// <typeparam name="T">The type of elements in the ICollection.</typeparam>
    /// <param name="collection">The ICollection to reset and add items to.</param>
    /// <param name="sourceNewItems">The IEnumerable of items to add.</param>
    public static void ResetAndAddRange<T>(this ICollection<T> collection, IEnumerable<T> sourceNewItems)
    {
        if (collection is null || sourceNewItems?.Any() != true) 
            return;
        var list = sourceNewItems.ToList();
        collection.Clear();
        list.ForEach(item =>
        {
            if (item != null)
                collection.Add(item);
        });
    }
}
